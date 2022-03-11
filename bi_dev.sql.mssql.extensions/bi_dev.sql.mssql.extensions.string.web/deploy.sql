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
		 ('@system_net_http_location', N'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.Http.dll')
		,('@build_location', N'C:\storage\hdd01\proj\sql\bi_dev.sql.mssql.extensions')
	) t (name, value);
	go
-- INPUT --
declare @system_net_http_location nvarchar(4000);
select
	 @system_net_http_location = t.value
from #t_tmp_var t
where t.name = '@system_net_http_location';
if (not exists (select 1 from sys.assemblies where name = N'System.Net.Http')) create ASSEMBLY [System.Net.Http] from @system_net_http_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [System.Net.Http] from @system_net_http_location with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
go
-- determin project-location
declare @build_location nvarchar(max);
select
	 @build_location = t.value
from #t_tmp_var t
where t.name = '@build_location'
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.string.web\bin\Debug\bi_dev.sql.mssql.extensions.@string.web.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_html_get_query_param_value') is not null) drop function dbo.f_clr_html_get_query_param_value;
	if (object_id('.dbo.f_clr_html_get_elements') is not null) drop function dbo.f_clr_html_get_elements;
	if (object_id('.dbo.f_clr_html_get_element_attribute_value') is not null) drop function dbo.f_clr_html_get_element_attribute_value;
	if (object_id('.dbo.f_clr_html_get_element_inner_text') is not null) drop function dbo.f_clr_html_get_element_inner_text;
	if (object_id('.dbo.f_clr_html_encode') is not null) drop function dbo.f_clr_html_encode;
	if (object_id('.dbo.f_clr_html_decode') is not null) drop function dbo.f_clr_html_decode;
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
	create function dbo.f_clr_html_encode(@value nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string.web].[bi_dev.sql.mssql.extensions.string.web.Utils].HtmlEncode;
	go
	create function dbo.f_clr_html_decode(@value nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string.web].[bi_dev.sql.mssql.extensions.string.web.Utils].HtmlDecode;
	go