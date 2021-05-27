-- INPUT --
-- determin project-location
declare @build_location nvarchar(max) = N'C:\storage\ssd01\app\sql\bi_dev.sql.mssql.extensions';
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.math\bin\Debug\bi_dev.sql.mssql.extensions.math.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_math_max_of_two') is not null) drop function dbo.f_clr_math_max_of_two;
	if (object_id('.dbo.f_clr_math_min_of_two') is not null) drop function dbo.f_clr_math_min_of_two;
	if (object_id('.dbo.f_clr_math_avg_of_two') is not null) drop function dbo.f_clr_math_avg_of_two;
	
	if (object_id('.dbo.f_clr_math_max_of_three') is not null) drop function dbo.f_clr_math_max_of_three;
	if (object_id('.dbo.f_clr_math_min_of_three') is not null) drop function dbo.f_clr_math_min_of_three;
	if (object_id('.dbo.f_clr_math_avg_of_three') is not null) drop function dbo.f_clr_math_avg_of_three;
	
	if (object_id('.dbo.f_clr_math_max_of_four') is not null) drop function dbo.f_clr_math_max_of_four;
	if (object_id('.dbo.f_clr_math_min_of_four') is not null) drop function dbo.f_clr_math_min_of_four;
	if (object_id('.dbo.f_clr_math_avg_of_four') is not null) drop function dbo.f_clr_math_avg_of_four;
	
	if (object_id('.dbo.f_clr_math_max_of_five') is not null) drop function dbo.f_clr_math_max_of_five;
	if (object_id('.dbo.f_clr_math_min_of_five') is not null) drop function dbo.f_clr_math_min_of_five;
	if (object_id('.dbo.f_clr_math_avg_of_five') is not null) drop function dbo.f_clr_math_avg_of_five;
	
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.math')) drop assembly [mssql.extensions.math];
-- create new --------------------
	create assembly [mssql.extensions.math] from @build_file_name with permission_set = unsafe;
	go
	create function dbo.f_clr_math_max_of_two(@a float, @b float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MaxOfTwo;
	go
	create function dbo.f_clr_math_min_of_two(@a float, @b float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MinOfTwo;
	go
	create function dbo.f_clr_math_avg_of_two(@a float, @b float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].AvgOfTwo;
	
	go
	create function dbo.f_clr_math_max_of_three(@a float, @b float, @c float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MaxOfThree;
	go
	create function dbo.f_clr_math_min_of_three(@a float, @b float, @c float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MinOfThree;
	go
	create function dbo.f_clr_math_avg_of_three(@a float, @b float, @c float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].AvgOfThree;
	
	go
	create function dbo.f_clr_math_max_of_four(@a float, @b float, @c float, @d float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MaxOfFour;
	go
	create function dbo.f_clr_math_min_of_four(@a float, @b float, @c float, @d float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MinOfFour;
	go
	create function dbo.f_clr_math_avg_of_four(@a float, @b float, @c float, @d float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].AvgOfFour;
	
	go
	create function dbo.f_clr_math_max_of_five(@a float, @b float, @c float, @d float, @e float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MaxOfFive;
	go
	create function dbo.f_clr_math_min_of_five(@a float, @b float, @c float, @d float, @e float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].MinOfFive;
	go
	create function dbo.f_clr_math_avg_of_five(@a float, @b float, @c float, @d float, @e float, @null_when_error bit) returns float with execute as owner as external name [mssql.extensions.math].[bi_dev.sql.mssql.extensions.math.Utils].AvgOfFive;
	
	