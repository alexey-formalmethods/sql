-- input variables -----------------------------
	if (object_id('tempdb..#t_tmp_var') is not null) drop table #t_tmp_var;
	create table #t_tmp_var (name nvarchar(max), value nvarchar(max));
	insert into #t_tmp_var
	(
		name
	  , value
	)
	select
		 name, value
	from (values
		 ('@system_runtime_serialization_location', N'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Runtime.Serialization.dll')
		,('@build_location', N'C:\storage\hdd01\proj\sql\bi_dev.sql.mssql.extensions')
	) t (name, value);
	go
------ assembiles -------------
------ System.Runtime.Serialization -------------
	declare @system_runtime_serialization_location nvarchar(4000);
	select
		 @system_runtime_serialization_location = t.value
	from #t_tmp_var t
	where t.name = '@system_runtime_serialization_location';
	if (not exists (select 1 from sys.assemblies where name = N'System.Runtime.Serialization')) create ASSEMBLY [System.Runtime.Serialization] from @system_runtime_serialization_location with permission_set = unsafe;
	else begin
		begin try
			alter ASSEMBLY [System.Net.Http] from @system_runtime_serialization_location with permission_set = unsafe;
		end try
		begin catch
			declare @error_message nvarchar(max) = error_message();
			if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
				raiserror(@error_message, 16, 1);
			end
		end catch
	end
	go
-- start ---------------------------
	declare @build_location nvarchar(max);
	select
		 @build_location = t.value
	from #t_tmp_var t
	where t.name = '@build_location'
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.aggregation\bin\Debug\bi_dev.sql.mssql.extensions.aggregation.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_agg_median') is not null) drop aggregate dbo.f_clr_agg_median;
	if (object_id('.dbo.f_clr_agg_string') is not null) drop aggregate dbo.f_clr_agg_string;
	
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.aggregation')) drop assembly [mssql.extensions.aggregation];
-- create new --------------------
	create assembly [mssql.extensions.aggregation] from @build_file_name with permission_set = unsafe;
	go
	create aggregate dbo.f_clr_agg_median(@value float, @is_null_equal_to_zero bit) returns float external name [mssql.extensions.aggregation].[bi_dev.sql.mssql.extensions.aggregation.Utils.Median];
	go
	create aggregate dbo.f_clr_agg_string(@value nvarchar(max), @delimiter nvarchar(max)) returns nvarchar(max) external name [mssql.extensions.aggregation].[bi_dev.sql.mssql.extensions.aggregation.Utils.StringAgg];
	go
