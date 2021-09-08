-- System.IO.Compression.FileSystem --------
declare @system_io_compression_filesystem nvarchar(4000) = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.IO.Compression.FileSystem.dll';
if (not exists (select 1 from sys.assemblies where name = N'System.IO.Compression.FileSystem')) create ASSEMBLY [System.IO.Compression.FileSystem] from @system_io_compression_filesystem with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [System.IO.Compression.FileSystem] from @system_io_compression_filesystem with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
-- INPUT --
-- determin project-location
	declare @build_location nvarchar(max) = N'C:\storage\hdd01\app\sql\bi_dev.sql.mssql.extensions';
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.file.zip\bin\Debug\bi_dev.sql.mssql.extensions.file.zip.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_create_zip_from_directory') is not null) drop function dbo.f_clr_create_zip_from_directory;
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.file.zip')) drop assembly [mssql.extensions.file.zip];
-------------------------
	create assembly [mssql.extensions.file.zip] from @build_file_name with permission_set = unsafe;
	go
	create function f_clr_create_zip_from_directory(@directory_name nvarchar(max), @destination_file_name nvarchar(max), @false_when_error bit) returns bit with execute as owner as external name [mssql.extensions.file.zip].[bi_dev.sql.mssql.extensions.file.zip.Utils].CreateZipFromDirectory;
	go
