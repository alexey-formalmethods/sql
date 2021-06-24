-- INPUT --
-- determin project-location
declare @build_location nvarchar(max) = N'C:\storage\ssd01\app\sql\bi_dev.sql.mssql.extensions';
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.aggregation\bin\Debug\bi_dev.sql.mssql.extensions.aggregation.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_agg_median') is not null) drop function dbo.f_clr_agg_median;
	
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.aggregation')) drop assembly [mssql.extensions.aggregation];
-- create new --------------------
	create assembly [mssql.extensions.aggregation] from @build_file_name with permission_set = unsafe;
	go
	create aggregate dbo.f_clr_agg_median(@value float, @is_null_equal_to_zero bit) returns float external name [mssql.extensions.aggregation].[Median];
	