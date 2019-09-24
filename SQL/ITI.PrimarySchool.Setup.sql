if not exists(select * from sys.databases where [name] = 'PrimarySchool') create database PrimarySchool;
GO

use PrimarySchool;
GO

if not exists (select * from sys.schemas where [name] = 'ps') exec('create schema ps;');
GO

if exists(select *
          from sys.tables t inner join sys.schemas s on s.[schema_id] = t.[schema_id]
		  where t.[name] = 'tStudent' and s.[name] = 'ps')
begin
	drop table ps.tStudent;
end;

if exists(select *
          from sys.tables t inner join sys.schemas s on s.[schema_id] = t.[schema_id]
		  where t.[name] = 'tClass' and s.[name] = 'ps')
begin
	drop table ps.tClass;
end;

if exists(select *
          from sys.tables t inner join sys.schemas s on s.[schema_id] = t.[schema_id]
		  where t.[name] = 'tTeacher' and s.[name] = 'ps')
begin
	drop table ps.tTeacher;
end;

create table ps.tTeacher
(
	TeacherId int identity(0, 1),
	FirstName nvarchar(32) collate Latin1_General_100_CI_AS not null,
	LastName  nvarchar(32) collate Latin1_General_100_CI_AS not null,

	constraint PK_ps_tTeacher primary key(TeacherId),
	constraint CK_ps_tTeacher_Name check(len(FirstName) > 1 and len(LastName) > 1),
	constraint UK_ps_tTeacher unique(LastName, FirstName)
);

insert into ps.tTeacher(FirstName, LastName) values(N'N.A.', N'N.A.');

create table ps.tClass
(
	ClassId    int identity(0, 1),
	[Name]     nvarchar(8) collate Latin1_General_100_CI_AS not null,
	[Level]    varchar(3) collate Latin1_General_100_CI_AS not null,
	TeacherId  int not null,

	constraint PK_ps_tClass primary key(ClassId),
	constraint FK_ps_tClass foreign key(TeacherId) references ps.tTeacher(TeacherId),
	constraint CK_ps_tClass_Level check([Level] in ('CP', 'CE1', 'CE2', 'CM1', 'CM2')),
	constraint UK_ps_tClass_Name unique([Name])
);

create unique index IX_ps_tClass_TeacherId on ps.tClass(TeacherId) where TeacherId > 0;

insert into ps.tClass([Name], [Level], TeacherId) values(N'', 'CP', 0);

create table ps.tStudent
(
	StudentId int identity(0, 1),
	FirstName nvarchar(32) collate Latin1_General_100_CI_AS not null,
	LastName  nvarchar(32) collate Latin1_General_100_CI_AS not null,
	BirthDate datetime2 not null,
	ClassId   int not null,

	constraint PK_ps_tStudent primary key(StudentId),
	constraint FK_ps_tStudent_ClassId foreign key(ClassId) references ps.tClass(ClassId),
	constraint UK_ps_tStudent_Name unique(LastName, FirstName)
);

insert into ps.tStudent(FirstName, LastName, BirthDate, ClassId) values(N'', N'', '00010101', 0);
