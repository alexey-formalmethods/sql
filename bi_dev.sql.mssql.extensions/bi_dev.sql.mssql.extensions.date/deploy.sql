
-- INPUT --
-- determin project-location
declare @build_location nvarchar(max) = N'C:\storage\hdd01\proj\sql\bi_dev.sql.mssql.extensions';
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.date\bin\Debug\bi_dev.sql.mssql.extensions.date.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_date_local_to_unix_timestamp') is not null) drop function dbo.f_clr_date_local_to_unix_timestamp;
	if (object_id('.dbo.f_clr_date_to_unix_timestamp') is not null) drop function dbo.f_clr_date_to_unix_timestamp;
	if (object_id('.dbo.f_clr_date_from_unix_timestamp') is not null) drop function dbo.f_clr_date_from_unix_timestamp;
	if (object_id('.dbo.f_clr_date_from_unix_timestamp_to_local') is not null) drop function dbo.f_clr_date_from_unix_timestamp_to_local;
	if (object_id('.dbo.f_clr_date_get_week_day_num_rus') is not null) drop function dbo.f_clr_date_get_week_day_num_rus;
	if (object_id('.dbo.f_clr_date_utc_to_local') is not null) drop function dbo.f_clr_date_utc_to_local
	if (object_id('.dbo.f_clr_date_min_of_two') is not null) drop function dbo.f_clr_date_min_of_two
	if (object_id('.dbo.f_clr_date_max_of_two') is not null) drop function dbo.f_clr_date_max_of_two
	if (object_id('.dbo.f_clr_date_from_string') is not null) drop function dbo.f_clr_date_from_string
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.date')) drop assembly [mssql.extensions.date];
-- create new --------------------
	create assembly [mssql.extensions.date] from @build_file_name with permission_set = unsafe;
	go
	create function dbo.f_clr_date_local_to_unix_timestamp(@value datetime, @include_milliseconds bit, @null_when_error bit) returns bigint with execute as owner as external name [mssql.extensions.date].[bi_dev.sql.mssql.extensions.date.Utils].LocalDateToUnixTimestamp;
	go
	create function dbo.f_clr_date_to_unix_timestamp(@value datetime, @include_milliseconds bit, @null_when_error bit) returns bigint with execute as owner as external name [mssql.extensions.date].[bi_dev.sql.mssql.extensions.date.Utils].DateToUnixTimestamp;
	go
	create function dbo.f_clr_date_from_unix_timestamp(@value bigint, @include_milliseconds bit, @null_when_error bit) returns datetime with execute as owner as external name [mssql.extensions.date].[bi_dev.sql.mssql.extensions.date.Utils].UnixTimestampToDate;
	go
	create function dbo.f_clr_date_from_unix_timestamp_to_local(@value bigint, @include_milliseconds bit, @null_when_error bit) returns datetime with execute as owner as external name [mssql.extensions.date].[bi_dev.sql.mssql.extensions.date.Utils].UnixTimestampToLocalDate;
	go
	create function dbo.f_clr_date_get_week_day_num_rus(@value datetime, @null_when_error bit) returns int with execute as owner as external name [mssql.extensions.date].[bi_dev.sql.mssql.extensions.date.Utils].GetWeekDayNumRus;
	go
	create function dbo.f_clr_date_utc_to_local(@value datetime, @null_when_error bit) returns datetime with execute as owner as external name [mssql.extensions.date].[bi_dev.sql.mssql.extensions.date.Utils].UtcDateToLocalDate;
	go
	create function dbo.f_clr_date_min_of_two(@value1 datetime, @valu2 datetime, @null_when_error bit) returns datetime with execute as owner as external name [mssql.extensions.date].[bi_dev.sql.mssql.extensions.date.Utils].MinOfTwo;
	go
	create function dbo.f_clr_date_max_of_two(@value1 datetime, @valu2 datetime, @null_when_error bit) returns datetime with execute as owner as external name [mssql.extensions.date].[bi_dev.sql.mssql.extensions.date.Utils].MaxOfTwo;
	go
	create function dbo.f_clr_date_from_string(@value nvarchar(max), @formats_array nvarchar(max), @null_when_error bit) returns datetime2 with execute as owner as external name [mssql.extensions.date].[bi_dev.sql.mssql.extensions.date.Utils].FromString;
	go