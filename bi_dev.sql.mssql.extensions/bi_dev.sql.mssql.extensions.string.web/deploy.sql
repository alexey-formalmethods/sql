-- INPUT --
-- determin project-location
declare @build_location nvarchar(max) = N'C:\storage\ssd01\app\sql\bi_dev.sql.mssql.extensions';
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.string.web\bin\Debug\bi_dev.sql.mssql.extensions.@string.web.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_html_get_query_param_value') is not null) drop function dbo.f_clr_html_get_query_param_value;
	if (object_id('.dbo.f_clr_html_get_elements') is not null) drop function dbo.f_clr_html_get_elements;
	if (object_id('.dbo.f_clr_html_get_element_attribute_value') is not null) drop function dbo.f_clr_html_get_element_attribute_value;
	if (object_id('.dbo.f_clr_html_get_element_inner_text') is not null) drop function dbo.f_clr_html_get_element_inner_text;
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.string.web')) drop assembly [mssql.extensions.string.web];
-- create new --------------------
	create assembly [mssql.extensions.string.web] from @build_file_name with permission_set = unsafe;
	go
	create function dbo.f_clr_html_get_query_param_value(@value nvarchar(max), @param_name nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string.web].[bi_dev.sql.mssql.extensions.string.web.Utils].GetQueryParamValue;
	go
	create function dbo.f_clr_html_get_elements(@value nvarchar(max), @x_path nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string.web].[bi_dev.sql.mssql.extensions.string.web.Utils].GetHtmlElements;
	go
	create function dbo.f_clr_html_get_element_attribute_value(@value nvarchar(max), @attribute_name nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string.web].[bi_dev.sql.mssql.extensions.string.web.Utils].GetHtmlElementAttributeValue;
	go
	create function dbo.f_clr_html_get_element_inner_text(@value nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string.web].[bi_dev.sql.mssql.extensions.string.web.Utils].GetHtmlElementInnerText;
	go