-- INPUT --
-- determin project-location
declare @build_location nvarchar(max) = N'C:\storage\ssd01\app\sql\bi_dev.sql.mssql.extensions';
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