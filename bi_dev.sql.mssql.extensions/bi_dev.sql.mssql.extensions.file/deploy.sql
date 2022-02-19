-- INPUT --
-- determin project-location
declare @build_location nvarchar(max) = N'C:\storage\ssd01\app\sql\bi_dev.sql.mssql.extensions';
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.file\bin\Debug\bi_dev.sql.mssql.extensions.file.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_get_file_content') is not null) drop function dbo.f_clr_get_file_content;
	if (object_id('.dbo.f_clr_check_if_path_exists') is not null) drop function dbo.f_clr_check_if_path_exists;
	if (object_id('.dbo.f_clr_delete_file') is not null) drop function dbo.f_clr_delete_file;
	if (object_id('.dbo.f_clr_write_text_to_file') is not null) drop function dbo.f_clr_write_text_to_file;
	if (object_id('.dbo.f_clr_write_text_to_file_no_bom') is not null) drop function dbo.f_clr_write_text_to_file_no_bom;
	if (object_id('.dbo.f_clr_copy_file') is not null) drop function dbo.f_clr_copy_file;
	if (object_id('.dbo.f_clr_filesystem_create_directory') is not null) drop function dbo.f_clr_filesystem_create_directory;
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.file')) drop assembly [mssql.extensions.file];
-- create new --------------------
	create assembly [mssql.extensions.file] from @build_file_name with permission_set = unsafe;
	go
	create function dbo.f_clr_get_file_content(@file_name nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.file].[bi_dev.sql.mssql.extensions.file.Utils].GetFileContent;
	go
	create function dbo.f_clr_check_if_path_exists(@path nvarchar(max), @null_when_error bit) returns bit with execute as owner as external name [mssql.extensions.file].[bi_dev.sql.mssql.extensions.file.Utils].[Exists];
	go
	create function dbo.f_clr_delete_file(@path nvarchar(max), @null_when_error bit) returns bit with execute as owner as external name [mssql.extensions.file].[bi_dev.sql.mssql.extensions.file.Utils].[Delete];
	go
	create function dbo.f_clr_write_text_to_file(@value nvarchar(max),@file_name nvarchar(max), @null_when_error bit) returns bigint with execute as owner as external name [mssql.extensions.file].[bi_dev.sql.mssql.extensions.file.Utils].[WriteTextToFile];
	go
	create function dbo.f_clr_write_text_to_file_no_bom(@value nvarchar(max),@file_name nvarchar(max), @null_when_error bit) returns bigint with execute as owner as external name [mssql.extensions.file].[bi_dev.sql.mssql.extensions.file.Utils].[WriteTextToFileNoBom];
	go
	create function f_clr_copy_file(@source_file_name nvarchar(max), @dest_file_name nvarchar(max), @overwrite bit, @null_when_error bit) returns bit with execute as owner as external name [mssql.extensions.file].[bi_dev.sql.mssql.extensions.file.Utils].[CopyFile];
	go
	create function f_clr_filesystem_create_directory(@directory_name nvarchar(max), @false_when_error bit) returns bit with execute as owner as external name [mssql.extensions.file].[bi_dev.sql.mssql.extensions.file.Utils].[CreateDirectory];
	go