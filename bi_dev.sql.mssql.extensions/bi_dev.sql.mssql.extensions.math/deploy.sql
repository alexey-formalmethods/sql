-- INPUT --
-- determin project-location
declare @build_location nvarchar(max) = N'C:\storage\ssd01\app\sql\bi_dev.sql.mssql.extensions';
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.math\bin\Debug\bi_dev.sql.mssql.extensions.math.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_math_max_of_five') is not null) drop function dbo.f_clr_math_max_of_five;
	if (object_id('.dbo.f_clr_math_min_of_five') is not null) drop function dbo.f_clr_math_min_of_five;
	if (object_id('.dbo.f_clr_math_avg_of_five') is not null) drop function dbo.f_clr_math_avg_of_five;
	
	if (object_id('.dbo.f_clr_math_max_of_five') is not null) drop function dbo.f_clr_math_max_of_five;
	if (object_id('.dbo.f_clr_math_min_of_five') is not null) drop function dbo.f_clr_math_min_of_five;
	if (object_id('.dbo.f_clr_math_avg_of_five') is not null) drop function dbo.f_clr_math_avg_of_five;
	
	if (object_id('.dbo.f_clr_math_max_of_five') is not null) drop function dbo.f_clr_math_max_of_five;
	if (object_id('.dbo.f_clr_math_min_of_five') is not null) drop function dbo.f_clr_math_min_of_five;
	if (object_id('.dbo.f_clr_math_avg_of_five') is not null) drop function dbo.f_clr_math_avg_of_five;
	
	if (object_id('.dbo.f_clr_math_max_of_five') is not null) drop function dbo.f_clr_math_max_of_five;
	if (object_id('.dbo.f_clr_math_min_of_five') is not null) drop function dbo.f_clr_math_min_of_five;
	if (object_id('.dbo.f_clr_math_avg_of_five') is not null) drop function dbo.f_clr_math_avg_of_five;
	
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.math')) drop assembly [mssql.extensions.math];
-- create new --------------------
	create assembly [mssql.extensions.math] from @build_file_name with permission_set = unsafe;
	go
	create function dbo.f_clr_math_max_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MaxOfFive;
	go
	create function dbo.f_clr_math_min_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MinOfFive;
	go
	create function dbo.f_clr_math_avg_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].AvgOfFive;
	
	go
	create function dbo.f_clr_math_max_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MaxOfFive;
	go
	create function dbo.f_clr_math_min_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MinOfFive;
	go
	create function dbo.f_clr_math_avg_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].AvgOfFive;
	
	go
	create function dbo.f_clr_math_max_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MaxOfFive;
	go
	create function dbo.f_clr_math_min_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MinOfFive;
	go
	create function dbo.f_clr_math_avg_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].AvgOfFive;
	
	go
	create function dbo.f_clr_math_max_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MaxOfFive;
	go
	create function dbo.f_clr_math_min_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MinOfFive;
	go
	create function dbo.f_clr_math_avg_of_five(float @a, float @b, float @c, float @d, float @e, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].AvgOfFive;
	
	