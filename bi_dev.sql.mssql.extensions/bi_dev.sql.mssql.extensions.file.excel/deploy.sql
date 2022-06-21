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
		 ('@build_location', N'C:\storage\hdd01\proj\source\sql\bi_dev.sql.mssql.extensions')
	) t (name, value);
	go

----------------------------
-- INPUT --
-- determin project-location
declare @build_location nvarchar(max);
select
	 @build_location = value
from #t_tmp_var
where name = '@build_location'
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.file.excel\bin\Debug\bi_dev.sql.mssql.extensions.file.excel.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_excel_get_content') is not null) drop function dbo.f_clr_excel_get_content
	if (object_id('.dbo.f_clr_excel_get_contents') is not null) drop function dbo.f_clr_excel_get_contents
	if (object_id('.dbo.f_clr_excel_get_sheets') is not null) drop function dbo.f_clr_excel_get_sheets
	if (exists (select 1 from sys.assemblies where name = N'bi_dev.sql.mssql.extensions.file.excel')) drop assembly [bi_dev.sql.mssql.extensions.file.excel];
-- create new --------------------
	create assembly [bi_dev.sql.mssql.extensions.file.excel] from @build_file_name with permission_set = unsafe;
	go
	create function dbo.f_clr_excel_get_content(@excel_sheet_request_json nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [bi_dev.sql.mssql.extensions.file.excel].[bi_dev.sql.mssql.extensions.file.excel.Utils].GetContent;
	go
	create function dbo.f_clr_excel_get_contents(@excel_request_json nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [bi_dev.sql.mssql.extensions.file.excel].[bi_dev.sql.mssql.extensions.file.excel.Utils].GetContents;
	go
	create function dbo.f_clr_excel_get_sheets(@file_name nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [bi_dev.sql.mssql.extensions.file.excel].[bi_dev.sql.mssql.extensions.file.excel.Utils].GetSheets;
	go
