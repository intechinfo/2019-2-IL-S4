if not exists(select * from sys.databases where [name] = 'PrimarySchool') create database PrimarySchool;
GO

use PrimarySchool;
GO

if not exists (select * from sys.schemas where [name] = 'ps') exec('create schema ps;');
GO

if exists(select *
          from sys.views v
          	inner join sys.schemas s on s.[schema_id] = v.[schema_id]
          where v.[name] = 'vStudent' and s.[name] = 'ps')
begin
	drop view ps.vStudent;
end;

if exists(select *
          from sys.views v
          	inner join sys.schemas s on s.[schema_id] = v.[schema_id]
          where v.[name] = 'vTeacher' and s.[name] = 'ps')
begin
	drop view ps.vTeacher;
end;

if exists(select *
          from sys.procedures p
          	inner join sys.schemas s on s.[schema_id] = p.[schema_id]
          where p.[Name] = 'sStudentCreate' and s.[name] = 'ps')
begin
	drop proc ps.sStudentCreate;
end;

if exists(select *
          from sys.procedures p
          	inner join sys.schemas s on s.[schema_id] = p.[schema_id]
          where p.[Name] = 'sClassCreate' and s.[name] = 'ps')
begin
	drop proc ps.sClassCreate;
end;

if exists(select *
          from sys.procedures p
          	inner join sys.schemas s on s.[schema_id] = p.[schema_id]
          where p.[Name] = 'sStudentAssignClass' and s.[name] = 'ps')
begin
	drop proc ps.sStudentAssignClass;
end;

if exists(select *
          from sys.procedures p
          	inner join sys.schemas s on s.[schema_id] = p.[schema_id]
          where p.[Name] = 'sStudentDelete' and s.[name] = 'ps')
begin
	drop proc ps.sStudentDelete;
end;

if exists(select *
          from sys.procedures p
          	inner join sys.schemas s on s.[schema_id] = p.[schema_id]
          where p.[Name] = 'sClassDelete' and s.[name] = 'ps')
begin
	drop proc ps.sClassDelete;
end;

if exists(select *
          from sys.procedures p
          	inner join sys.schemas s on s.[schema_id] = p.[schema_id]
          where p.[Name] = 'sTeacherAssignClass' and s.[name] = 'ps')
begin
	drop proc ps.sTeacherAssignClass;
end;

if exists(select *
          from sys.procedures p
          	inner join sys.schemas s on s.[schema_id] = p.[schema_id]
          where p.[Name] = 'sTeacherCreate' and s.[name] = 'ps')
begin
	drop proc ps.sTeacherCreate;
end;

if exists(select *
          from sys.procedures p
          	inner join sys.schemas s on s.[schema_id] = p.[schema_id]
          where p.[Name] = 'sTeacherDelete' and s.[name] = 'ps')
begin
	drop proc ps.sTeacherDelete;
end;

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
GO

create view ps.vStudent
as
	select
		StudentId = s.StudentId,
		FirstName = s.FirstName,
		LastName = s.LastName,
		BirthDate = s.BirthDate,
		ClassId = c.ClassId,
		ClassName = case when c.ClassId <> 0 then c.[Name] else N'' end,
		[Level] = case when c.ClassId <> 0 then c.[Level] else '' end,
		TeacherId = t.TeacherId,
		TeacherFirstName = case when t.TeacherId <> 0 then t.FirstName else N'' end,
		TeacherLastName = case when t.TeacherId <> 0 then t.LastName else N'' end
	from ps.tStudent s
		inner join ps.tClass c on c.ClassId = s.ClassId
		inner join ps.tTeacher t on t.TeacherId = c.TeacherId
	where s.StudentId <> 0;
GO

create proc ps.sStudentCreate
(
	@FirstName nvarchar(32),
	@LastName nvarchar(32),
	@BirthDate datetime2,
	@StudentId int out
)
as
begin
	set xact_abort on;
	set transaction isolation level serializable;

	begin tran;

	if exists(select * from ps.tStudent s where s.FirstName = @FirstName and s.LastName = @LastName)
	begin
		rollback;
		return 1;
	end;

	insert into ps.tStudent(FirstName,  LastName,  BirthDate,  ClassId)
	                 values(@FirstName, @LastName, @BirthDate, 0);

	set @StudentId = scope_identity();
	commit;
	return 0;
end;
GO

create proc ps.sClassCreate
(
	@Name nvarchar(8),
	@Level varchar(3),
	@ClassId int out
)
as
begin
	set xact_abort on;
	set transaction isolation level serializable;

	begin tran;

	if @Level not in('CP', 'CE1', 'CE2', 'CM1', 'CM2')
	begin
		rollback;
		return 1;
	end;

	if exists(select * from ps.tClass c where c.[Name] = @Name)
	begin
		rollback;
		return 2;
	end;

	insert into ps.tClass([Name], [Level], TeacherId)
	               values(@Name,  @Level,  0);

	set @ClassId = scope_identity();

	commit;
	return 0;
end;
GO

create proc ps.sStudentAssignClass
(
	@StudentId int,
	@ClassId int
)
as
begin
	set xact_abort on;
	set transaction isolation level serializable;

	begin tran;
	if not exists(select * from ps.tClass c where c.ClassId = @ClassId)
	begin
		rollback;
		return 1;
	end;

	if not exists(select * from ps.tStudent s where s.StudentId = @StudentId)
	begin
		rollback;
		return 2;
	end;

	update ps.tStudent set ClassId = @ClassId where StudentId = @StudentId;

	commit;
	return 0;
end;
GO

create proc ps.sStudentDelete
(
	@StudentId int
)
as
begin
	set xact_abort on;
	set transaction isolation level serializable;

	begin tran;

	if not exists(select * from ps.tStudent s where s.StudentId = @StudentId)
	begin
		rollback;
		return 1;
	end;

	delete from ps.tStudent where StudentId = @StudentId;

	commit;
	return 0;
end;
GO

create proc ps.sClassDelete
(
	@ClassId int
)
as
begin
	set xact_abort on;
	set transaction isolation level serializable;

	begin tran;

	if not exists(select * from ps.tClass c where c.ClassId = @ClassId)
	begin
		rollback;
		return 1;
	end;

	update ps.tStudent set ClassId = 0 where ClassId = @ClassId;
	delete ps.tClass where ClassId = @ClassId;
	commit;

	return 0;
end;
GO

create proc ps.sTeacherCreate
(
	@FirstName nvarchar(32),
	@LastName nvarchar(32),
	@TeacherId int out
)
as
begin
	set xact_abort on;
	set transaction isolation level serializable;

	begin tran;

	if exists(select * from ps.tTeacher t where t.FirstName = @FirstName and t.LastName = @LastName)
	begin
		rollback;
		return 1;
	end;

	insert into ps.tTeacher(FirstName, LastName) values(@FirstName, @LastName);
	set @TeacherId = scope_identity();

	commit;
	return 0;
end;
GO

create proc ps.sTeacherAssignClass
(
	@TeacherId int,
	@ClassId int
)
as
begin
	set xact_abort on;
	set transaction isolation level serializable;

	begin tran;

	if not exists(select * from ps.tTeacher t where t.TeacherId = @TeacherId)
	begin
		rollback;
		return 1;
	end;

	if not exists(select * from ps.tClass c where c.ClassId = @ClassId)
	begin
		rollback;
		return 2;
	end;

	update ps.tClass set TeacherId = 0 where TeacherId = @TeacherId;
	update ps.tClass set TeacherId = @TeacherId where ClassId = @ClassId;

	commit;
	return 0;
end;
GO

create view ps.vTeacher
as
	select
		TeacherId = t.TeacherId,
		FirstName = t.FirstName,
		LastName = t.LastName,
		ClassId = coalesce(c.ClassId, 0),
		ClassName = coalesce(c.[Name], N''),
		ClassLevel = coalesce(c.[Level], '')
	from ps.tTeacher t
		left outer join ps.tClass c on c.TeacherId = t.TeacherId
	where t.TeacherId <> 0;
GO

create proc ps.sTeacherDelete
(
	@TeacherId int
)
as
begin
	set xact_abort on;
	set transaction isolation level serializable;

	begin tran;

	if not exists(select * from ps.tTeacher t where t.TeacherId = @TeacherId)
	begin
		rollback;
		return 1;
	end;

	update ps.tClass set TeacherId = 0 where TeacherId = @TeacherId;
	delete ps.tTeacher where TeacherId = @TeacherId;
	commit;

	return 0;
end;