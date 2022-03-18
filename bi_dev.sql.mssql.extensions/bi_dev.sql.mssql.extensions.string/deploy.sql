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
		 ('@build_location', N'C:\storage\hdd01\proj\sql\bi_dev.sql.mssql.extensions')
	) t (name, value);
	go
-- INPUT --
-- determin project-location
declare @build_location nvarchar(max);
select
	 @build_location = t.value
from #t_tmp_var t
where t.name = '@build_location';
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.string\bin\Debug\bi_dev.sql.mssql.extensions.@string.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_regex_match') is not null) drop function dbo.f_clr_regex_match;
	if (object_id('.dbo.f_clr_split_string') is not null) drop function dbo.f_clr_split_string;
	if (object_id('.dbo.f_clr_remove_non_digits') is not null) drop function dbo.f_clr_remove_non_digits;
	if (object_id('.dbo.f_clr_unicode_decode') is not null) drop function dbo.f_clr_unicode_decode;
	if (object_id('.dbo.f_clr_regex_matches') is not null) drop function dbo.f_clr_regex_matches;
	if (object_id('.dbo.f_clr_url_encode') is not null) drop function dbo.f_clr_url_encode;
	if (object_id('.dbo.f_clr_url_decode') is not null) drop function dbo.f_clr_url_decode;
	if (object_id('.dbo.f_clr_base64_encode') is not null) drop function dbo.f_clr_base64_encode;
	if (object_id('.dbo.f_clr_base64_decode') is not null) drop function dbo.f_clr_base64_decode;
	if (object_id('.dbo.f_clr_get_sha_256_hash') is not null) drop function dbo.f_clr_get_sha_256_hash;
	if (object_id('.dbo.f_clr_string_replace') is not null) drop function dbo.f_clr_string_replace;
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.string')) drop assembly [mssql.extensions.string];
-- create new --------------------
	create assembly [mssql.extensions.string] from @build_file_name with permission_set = unsafe;
	go
	create function dbo.f_clr_regex_match(@value nvarchar(max), @regex_pattern nvarchar(max), @group_number int,  @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].RegexMatch;
	go
	create function dbo.f_clr_split_string(@value nvarchar(max), @separaor nvarchar(max), @index int,  @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].SplitString
	go
	create function dbo.f_clr_remove_non_digits(@value nvarchar(max), @null_when_error bit) returns bigint with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].RemoveNonDigits 
	go
	create function dbo.f_clr_unicode_decode(@value nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].UnicodeDecode 
	go
	create function dbo.f_clr_regex_matches(@value nvarchar(max), @regex_pattern nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].RegexMatches;
	go
	create function f_clr_url_encode(@value nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].UrlEncode;
	go
	create function f_clr_url_decode(@value nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].UrlDecode;
	go
	create function f_clr_base64_encode(@value nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].Base64Encode;
	go
	create function f_clr_base64_decode(@value nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].Base64Decode;
	go
	create function f_clr_get_sha_256_hash(@value nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].GetSha256Hash;
	go
	create function f_clr_string_replace(@value nvarchar(max), @values_to_replace nvarchar(max), @replace_value nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string].[bi_dev.sql.mssql.extensions.string.Utils].[Replace];
	go
