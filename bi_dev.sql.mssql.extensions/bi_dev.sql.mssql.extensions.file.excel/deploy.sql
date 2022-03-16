drop function f_clr_get_excel_content
drop assembly [mssql.extensions.file.excel]
create assembly [mssql.extensions.file.excel] from  N'C:\storage\hdd01\proj\sql\bi_dev.sql.mssql.extensions\bi_dev.sql.mssql.extensions.file.excel\bin\Debug\bi_dev.sql.mssql.extensions.file.excel.dll' with 
permission_set = unsafe;
go
create function dbo.f_clr_get_excel_content(
	@file_name nvarchar(max),
	@sheet_name nvarchar(max),
	@row_number_from int,
	@row_number_to int,
	@column_number_from int,
	@column_number_to int,
	@is_first_row_with_column_names bit,
	@null_when_error bit
) returns nvarchar(max)
with execute as owner as external name [mssql.extensions.file.excel].[bi_dev.sql.mssql.extensions.file.excel.Utils].GetExcelContent;

create function dbo.f_clr_get_excel_content_unformatted(
	@file_name nvarchar(max),
	@sheet_name nvarchar(max),
	@row_number_from int,
	@row_number_to int,
	@column_number_from int,
	@column_number_to int,
	@is_first_row_with_column_names bit,
	@null_when_error bit
) returns nvarchar(max)
with execute as owner as external name [mssql.extensions.file.excel].[bi_dev.sql.mssql.extensions.file.excel.Utils].GetExcelContentUnformatted;