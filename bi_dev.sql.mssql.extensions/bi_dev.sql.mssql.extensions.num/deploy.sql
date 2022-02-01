-- INPUT --
-- determin project-location
declare @build_location nvarchar(max) = N'C:\storage\hdd01\proj\sql\bi_dev.sql.mssql.extensions';
-------------------------------
declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.num\bin\Debug\bi_dev.sql.mssql.extensions.num.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_num_to_rub') is not null) drop function dbo.f_clr_num_to_rub;
	
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.num')) drop assembly [mssql.extensions.num];
-- create new --------------------
	create assembly [mssql.extensions.num] from @build_file_name with permission_set = unsafe;
	go
	create function dbo.f_clr_num_to_rub(@value bigint, @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.num].[bi_dev.sql.mssql.extensions.num.Utils].ToRub;
	go
