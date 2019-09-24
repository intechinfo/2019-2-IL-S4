if not exists(select * from sys.databases where [name] = 'PrimarySchool') create database PrimarySchool;
GO

use PrimarySchool;
GO

if not exists (select * from sys.schemas where [name] = 'ps') exec('create schema ps;');
GO

if exists (select *
           from sys.tables t
		       inner join sys.schemas s on s.schema_id = t.schema_id
		   where t.[name] = 'tSchool' and s.[name] = 'ps')
begin
	drop table ps.tSchool;
end;

create table ps.tSchool
(
	EntityId  int identity(0, 1),
	FirstName nvarchar(32) collate Latin1_General_100_CI_AS,
	LastName  nvarchar(32) collate Latin1_General_100_CI_AS,
	BirthDate datetime2,
	ClassId   int,
	ClassName nvarchar(8) collate Latin1_General_100_CI_AS,
	[Level]   varchar(3) collate Latin1_General_100_CI_AS,
	TeacherId int,

	constraint PK_ps_tSchool primary key(EntityId),
	constraint FK_ps_tSchool_ClassId foreign key(ClassId) references ps.tSchool(EntityId),
	constraint FK_ps_tSchool_TeacherId foreign key(TeacherId) references ps.tSchool(EntityId),
	constraint CK_ps_tSchool_Level check([Level] in ('CP', 'CE1', 'CE2', 'CM1', 'CM2'))
);


