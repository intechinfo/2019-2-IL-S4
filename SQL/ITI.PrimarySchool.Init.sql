use PrimarySchool;

insert into ps.tSchool(FirstName, LastName) values(N'TeacherFirstName0', N'TeacherLastName0'),
                                                  (N'TeacherFirstName1', N'TeacherLastName1'),
                                                  (N'TeacherFirstName2', N'TeacherLastName2'),
                                                  (N'TeacherFirstName3', N'TeacherLastName3'),
                                                  (N'TeacherFirstName4', N'TeacherLastName4');

insert into ps.tSchool(ClassName, [Level], TeacherId) values(N'CP-1',  'CP', 0),
                                                            (N'CE1-1', 'CE1', null),
                                                            (N'CE2-1', 'CE2', 2),
                                                            (N'CM1-1', 'CM1', 3),
                                                            (N'CM2-1', 'CM2', 4);

insert into ps.tSchool(FirstName, LastName, BirthDate, ClassId) values(N'StudentFirstName0',  N'StudentLastName0',  '20090101', 5),
                                                                      (N'StudentFirstName1',  N'StudentLastName1',  '20090201', 5),
                                                                      (N'StudentFirstName2',  N'StudentLastName2',  '20090301', 5),
                                                                      (N'StudentFirstName3',  N'StudentLastName3',  '20080101', 6),
                                                                      (N'StudentFirstName4',  N'StudentLastName4',  '20080201', 6),
                                                                      (N'StudentFirstName5',  N'StudentLastName5',  '20080301', 6),
                                                                      (N'StudentFirstName6',  N'StudentLastName6',  '20070101', 7),
                                                                      (N'StudentFirstName7',  N'StudentLastName7',  '20070201', 7),
                                                                      (N'StudentFirstName8',  N'StudentLastName8',  '20070301', 7),
                                                                      (N'StudentFirstName9',  N'StudentLastName9',  '20060101', 8),
                                                                      (N'StudentFirstName10', N'StudentLastName10', '20060201', 8),
                                                                      (N'StudentFirstName11', N'StudentLastName11', '20060201', 8),
                                                                      (N'StudentFirstName12', N'StudentLastName12', '20050101', 9),
                                                                      (N'StudentFirstName13', N'StudentLastName13', '20050201', 9),
                                                                      (N'StudentFirstName14', N'StudentLastName14', '20050301', 9);
