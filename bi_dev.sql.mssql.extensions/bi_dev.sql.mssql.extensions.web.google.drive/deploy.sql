-- INPUT --
declare @build_location nvarchar(max) = N'C:\storage\hdd01\proj\sql\bi_dev.sql.mssql.extensions';
declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.web.google.drive\bin\Debug\bi_dev.sql.mssql.extensions.web.google.drive.dll';
-- drop existing --------
if (object_id('.dbo.f_clr_gd_get_files') is not null) drop function dbo.f_clr_gd_get_files;
if (object_id('.dbo.f_clr_gd_download_file') is not null) drop function dbo.f_clr_gd_download_file;
if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.web.google.drive')) drop ASSEMBLY [mssql.extensions.web.google.drive];
-- create new --------------------
create assembly [mssql.extensions.web.google.drive] from @build_file_name with permission_set = unsafe;
go
create function dbo.f_clr_gd_get_files (
	@access_token nvarchar(max),
	@null_when_error bit
) returns nvarchar(max)
with execute as owner
as external name [mssql.extensions.web.google.drive].[bi_dev.sql.mssql.extensions.web.google.drive.Utils].[GetFiles];
go
create function dbo.f_clr_gd_download_file (
	@access_token nvarchar(max),
	@file_id nvarchar(max),
	@file_name nvarchar(max),
	@null_when_error bit
) returns bit
with execute as owner
as external name [mssql.extensions.web.google.drive].[bi_dev.sql.mssql.extensions.web.google.drive.Utils].[DownloadFile];
go