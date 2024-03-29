﻿using Microsoft.EntityFrameworkCore;
using SergeyPetrovKT_31_20.Database;
using SergeyPetrovKT_31_20.interfaces.StudentInterfaces;
using SergeyPetrovKT_31_20.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group = SergeyPetrovKT_31_20.Models.Group;

namespace SergeyPetrovKT_31_20.Tests
{
    public class StudentIntegrationTests
    {
        public readonly DbContextOptions<SerDbContext> _dbContextOptions;
        public StudentIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<SerDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
        }

        public async Task GetStudentsByGroupAsync_KT3120_TwoObjects()
        {
            // Arrange
            var _dbContextOptions = new DbContextOptionsBuilder<SerDbContext>().Options;

            var ctx = new SerDbContext(_dbContextOptions);
            // var studentService = new IStudentService(ctx);
            var studentService = new StudentFilterService(ctx);
            var groups = new List<Group>
            {
                new Group
                {
                    GroupName = "KT-31-20"
                },
                new Group
                {
                    GroupName = "KT-41-20"
                }
            };
            await ctx.Set<Group>().AddRangeAsync(groups);

            var students = new List<Student>
            {
                new Student
                {
                    FirstName = "qwerty",
                    LastName = "asdf",
                    MiddleName = "zxc",
                    GroupId = 1,
                },
                new Student
                {
                    FirstName = "qwerty2",
                    LastName = "asdf2",
                    MiddleName = "zxc2",
                    GroupId = 2,
                },
                new Student
                {
                    FirstName = "qwerty3",
                    LastName = "asdf3",
                    MiddleName = "zxc3",
                    GroupId = 1,
                }
            };
            await ctx.Set<Student>().AddRangeAsync(students);

            await ctx.SaveChangesAsync();

            // Act
            var filter = new SergeyPetrovKT_31_20.Filters.StudentFilters.StudentGroupFilter
            {
                GroupName = "KT-31-20"
            };
            var studentsResult = await studentService.GetStudentsByGroupAsync(filter, CancellationToken.None);

            // Assert
            Assert.Equal(2, studentsResult.Length);
        }

        [Fact]
        public async Task GetStudentsAsync_AllStudents()
        {
            // Arrange
            var ctx = new SerDbContext(_dbContextOptions);
            var studentService = new StudentFilterService(ctx);
            var groups = new List<Group>
            {
                new Group
                {
                    GroupName = "KT-31-20"
                },
                new Group
                {
                    GroupName = "KT-41-20"
                }
            };
            await ctx.Set<Group>().AddRangeAsync(groups);

            var students = new List<Student>
            {
                new Student
                {
                   FirstName = "Petr",
                    LastName = "Petrov",
                    MiddleName = "Petrovich",

                    GroupId=1
                },
                new Student
                {
                    FirstName = "Николай",
                    LastName = "Бакин",
                    MiddleName = "Сергеевич",
                    GroupId = 1
                },
                new Student
                {
                   FirstName = "ivan",
                    LastName = "ivanov",
                    MiddleName = "ivanovich",
                    GroupId = 2
                }
            };
            await ctx.Set<Student>().AddRangeAsync(students);

            await ctx.SaveChangesAsync();

            // Act
            var studentsResult = await studentService.GetStudentsAsync(CancellationToken.None);

            // Assert
            Assert.Equal(5, studentsResult.Length);
        }

        [Fact]
        public async Task GetStudentAsync_Id_Student()
        {
            // Arrange
            var ctx = new SerDbContext(_dbContextOptions);
            var studentService = new StudentFilterService(ctx);
            var groups = new List<Group>
            {
                new Group
                {
                    GroupName = "KT-31-20"
                }
            };
            await ctx.Set<Group>().AddRangeAsync(groups);

            var student = new Student
            {
                FirstName = "Petr",
                LastName = "Petrov",
                MiddleName = "Petrovich",

                GroupId = 1
            };
            await ctx.Set<Student>().AddAsync(student);
            await ctx.SaveChangesAsync();

            // Act
            var studentResult = await studentService.GetStudentAsync(student.StudentId, CancellationToken.None);

            // Assert
            Assert.Equal(student.StudentId, studentResult.StudentId);
            Assert.Equal(student.FirstName, studentResult.FirstName);
            Assert.Equal(student.LastName, studentResult.LastName);
            Assert.Equal(student.MiddleName, studentResult.MiddleName);
            Assert.Equal(student.GroupId, studentResult.GroupId);
        }

        [Fact]
        public async Task AddStudentAsync_AddsStudent()
        {
            // Arrange
            var ctx = new SerDbContext(_dbContextOptions);
            var studentService = new StudentFilterService(ctx);


            var group = new Group
            {
                GroupName = "KT-41-20"
            };
            await ctx.Set<Group>().AddAsync(group);
            await ctx.SaveChangesAsync();

            var student = new Student
            {
                FirstName = "Александр",
                LastName = "Смирнов",
                MiddleName = "Иванович",

                GroupId = group.GroupId

            };

            // Act
            await studentService.AddStudentAsync(student);

            // Assert
            var addedStudent = await ctx.Set<Student>().SingleOrDefaultAsync(s => s.FirstName == "Александр" && s.LastName == "Смирнов");
            Assert.NotNull(addedStudent);
            Assert.Equal("Иванович", addedStudent.MiddleName);

            Assert.Equal(group.GroupId, addedStudent.GroupId);
        }

        [Fact]
        public async Task DeleteStudentAsync_Id_Student()
        {
            // Arrange
            var ctx = new SerDbContext(_dbContextOptions);
            var studentService = new StudentFilterService(ctx);
            var groups = new List<Group>
            {
                new Group
                {
                    GroupName = "KT-31-20"
                },
                 new Group
                {
                    GroupName = "KT-41-20"
                }
            };
            await ctx.Set<Group>().AddRangeAsync(groups);


            var students = new List<Student>
            {
                new Student
                {
                  FirstName = "Petr",
                    LastName = "Petrov",
                    MiddleName = "Petrovich",

                    GroupId=1
                },
                new Student
                {
                    FirstName = "Николай",
                    LastName = "Бакин",
                    MiddleName = "Сергеевич",
                    GroupId = 1
                },
                new Student
                {
                   FirstName = "ivan",
                    LastName = "ivanov",
                    MiddleName = "ivanovich",
                    GroupId = 2
                }
            };
            await ctx.Set<Student>().AddRangeAsync(students);

            await ctx.SaveChangesAsync();

            // Act
            var studentIdToDelete = 1; // ID of the student to delete
            var studentResult = await studentService.GetStudentAsync(studentIdToDelete, CancellationToken.None);
            await studentService.DeleteStudentAsync(studentResult, CancellationToken.None);

            var studentsResult = await studentService.GetStudentsAsync(CancellationToken.None);

            // Assert
            Assert.Equal(7, studentsResult.Length);
            Assert.DoesNotContain(studentsResult, s => s.StudentId == studentIdToDelete);
        }

        [Fact]
        public async Task UpdateStudentAsync_Id_Student()
        {
            // Arrange
            var ctx = new SerDbContext(_dbContextOptions);
            var studentService = new StudentFilterService(ctx);
            var student = new Student
            {
                FirstName = "Petr",
                LastName = "Petrov",
                MiddleName = "Petrovich",

                GroupId = 1
            };
            await ctx.Set<Student>().AddAsync(student);
            await ctx.SaveChangesAsync();

            // Act
            student.FirstName = "Никита";
            student.LastName = "Makarov";
            student.MiddleName = "Aleksandrovich";

            await studentService.UpdateStudentAsync(student, CancellationToken.None);

            // Assert
            Assert.Equal("Никита", student.FirstName);
        }

    }
}
