using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using DepartmentApp.Authorization;
using DepartmentApp.Entities;

namespace DepartmentApp.EntityFrameworkCore.Seed
{
    /// <summary>
    /// Seeds default RBAC data on first run:
    ///   - Admin role (IsSystem) granted ALL permissions from PermissionRegistry
    ///   - User role (IsSystem) granted only *.Read permissions
    ///   - admin/123qwe user assigned to Admin role
    /// Idempotent — safe to run on every startup.
    /// </summary>
    public class DefaultUserSeeder
    {
        private readonly IRepository<AppUser, long> _userRepo;
        private readonly IRepository<AppRole, long> _roleRepo;
        private readonly IRepository<UserRole, long> _userRoleRepo;
        private readonly IRepository<RolePermission, long> _rolePermRepo;
        private readonly IPermissionRegistry _permRegistry;

        public DefaultUserSeeder(
            IRepository<AppUser, long> userRepo,
            IRepository<AppRole, long> roleRepo,
            IRepository<UserRole, long> userRoleRepo,
            IRepository<RolePermission, long> rolePermRepo,
            IPermissionRegistry permRegistry)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _userRoleRepo = userRoleRepo;
            _rolePermRepo = rolePermRepo;
            _permRegistry = permRegistry;
        }

        public void Seed()
        {
            // Each step is logged so silent failures at any stage are immediately visible.
            try
            {
                Console.WriteLine("[Seed] Step 1/4: Admin role");
                var adminRole = _roleRepo.GetAll().FirstOrDefault(r => r.Name == "Admin");
                if (adminRole == null)
                {
                    adminRole = new AppRole { Name = "Admin", DisplayName = "Administrator", Description = "Full system access", IsSystem = true, IsActive = true };
                    adminRole.Id = _roleRepo.InsertAndGetId(adminRole);
                    Console.WriteLine($"[Seed] Created Admin role (id={adminRole.Id})");
                }

                Console.WriteLine("[Seed] Step 2/4: Grant all permissions to Admin");
                var allPermNames = _permRegistry.All.Select(p => p.Name).ToList();
                var adminGranted = _rolePermRepo.GetAll().Where(p => p.RoleId == adminRole.Id).Select(p => p.PermissionName).ToList();
                int granted = 0;
                foreach (var pn in allPermNames.Except(adminGranted))
                {
                    _rolePermRepo.Insert(new RolePermission { RoleId = adminRole.Id, PermissionName = pn });
                    granted++;
                }
                Console.WriteLine($"[Seed] Granted {granted} new permissions to Admin (total: {allPermNames.Count})");

                Console.WriteLine("[Seed] Step 3/4: User role + read-only permissions");
                var userRole = _roleRepo.GetAll().FirstOrDefault(r => r.Name == "User");
                if (userRole == null)
                {
                    userRole = new AppRole { Name = "User", DisplayName = "Standard User", Description = "Read-only access to business data", IsSystem = true, IsActive = true };
                    userRole.Id = _roleRepo.InsertAndGetId(userRole);
                    Console.WriteLine($"[Seed] Created User role (id={userRole.Id})");
                }
                var readOnlyPerms = _permRegistry.All.Where(p => !p.IsRbac && p.Name.EndsWith(".Read")).Select(p => p.Name).ToList();
                var userGranted = _rolePermRepo.GetAll().Where(p => p.RoleId == userRole.Id).Select(p => p.PermissionName).ToList();
                foreach (var pn in readOnlyPerms.Except(userGranted))
                    _rolePermRepo.Insert(new RolePermission { RoleId = userRole.Id, PermissionName = pn });

                Console.WriteLine("[Seed] Step 4/4: Admin user (admin/123qwe)");
                var adminUser = _userRepo.GetAll().FirstOrDefault(u => u.UserName == "admin");
                if (adminUser == null)
                {
                    adminUser = new AppUser
                    {
                        UserName = "admin", EmailAddress = "admin@example.com",
                        Name = "Admin", Surname = "User",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("123qwe"),
                        IsActive = true,
                    };
                    adminUser.Id = _userRepo.InsertAndGetId(adminUser);
                    Console.WriteLine($"[Seed] Created admin user (id={adminUser.Id})");
                }
                if (!_userRoleRepo.GetAll().Any(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id))
                {
                    _userRoleRepo.Insert(new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id });
                    Console.WriteLine("[Seed] Linked admin → Admin role");
                }

                Console.WriteLine("[Seed] Step 5: Custom role 'HRSpecialist'");
                var customRole_0 = _roleRepo.GetAll().FirstOrDefault(r => r.Name == "HRSpecialist");
                if (customRole_0 == null)
                {
                    customRole_0 = new AppRole { Name = "HRSpecialist", DisplayName = "İK Uzmanı", Description = "İnsan kaynakları uzmanı — izin onayları, işe alım süreçleri, maaş kayıtları ve eğitim planlarını yönetir", IsSystem = false, IsActive = true };
                    customRole_0.Id = _roleRepo.InsertAndGetId(customRole_0);
                    Console.WriteLine($"[Seed] Created custom role 'HRSpecialist' (id={customRole_0.Id})");
                }
                var customPerms_0 = new[] { "Employee.Read", "Employee.Create", "Employee.Update", "Employee.Delete", "Employee.ChangeStatus", "LeaveRequest.Read", "LeaveRequest.Create", "LeaveRequest.Update", "LeaveRequest.Delete", "LeaveRequest.ChangeStatus", "JobPosting.Read", "JobPosting.Create", "JobPosting.Update", "JobPosting.Delete", "JobPosting.ChangeStatus", "JobApplication.Read", "JobApplication.Create", "JobApplication.Update", "JobApplication.Delete", "JobApplication.ChangeStatus", "Onboarding.Read", "Onboarding.Create", "Onboarding.Update", "Onboarding.Delete", "Onboarding.ChangeStatus", "SalaryRecord.Read", "SalaryRecord.Create", "SalaryRecord.Update", "SalaryRecord.Delete", "SalaryRecord.ChangeStatus", "SalaryDeduction.Read", "SalaryDeduction.Create", "SalaryDeduction.Update", "SalaryDeduction.Delete", "SalaryDeduction.ChangeStatus", "TrainingPlan.Read", "TrainingPlan.Create", "TrainingPlan.Update", "TrainingPlan.Delete", "TrainingPlan.ChangeStatus", "Training.Read", "Training.Create", "Training.Update", "Training.Delete", "Training.ChangeStatus", "TrainingParticipation.Read", "TrainingParticipation.Create", "TrainingParticipation.Update", "TrainingParticipation.Delete", "TrainingParticipation.ChangeStatus", "PerformanceReview.Read", "PerformanceReview.Create", "PerformanceReview.Update", "PerformanceReview.Delete", "PerformanceReview.ChangeStatus", "EmployeeCertificate.Read", "EmployeeCertificate.Create", "EmployeeCertificate.Update", "EmployeeCertificate.Delete", "EmployeeCertificate.ChangeStatus", "LeaveType.Read", "LeaveType.Create", "LeaveType.Update", "LeaveType.Delete", "LeaveType.ChangeStatus", "DisciplinaryRecord.Read", "DisciplinaryRecord.Create", "DisciplinaryRecord.Update", "DisciplinaryRecord.Delete", "DisciplinaryRecord.ChangeStatus", "OvertimeRecord.Read", "OvertimeRecord.Create", "OvertimeRecord.Update", "OvertimeRecord.Delete", "OvertimeRecord.ChangeStatus" };
                var customGranted_0 = _rolePermRepo.GetAll().Where(p => p.RoleId == customRole_0.Id).Select(p => p.PermissionName).ToList();
                var allPermSet_0 = new HashSet<string>(_permRegistry.All.Select(p => p.Name));
                foreach (var pn in customPerms_0.Except(customGranted_0))
                {
                    if (!allPermSet_0.Contains(pn)) { Console.WriteLine($"[Seed] Skipping unknown permission '{pn}' for role 'HRSpecialist'"); continue; }
                    _rolePermRepo.Insert(new RolePermission { RoleId = customRole_0.Id, PermissionName = pn });
                }

                Console.WriteLine("[Seed] Step 6: Custom role 'HRManager'");
                var customRole_1 = _roleRepo.GetAll().FirstOrDefault(r => r.Name == "HRManager");
                if (customRole_1 == null)
                {
                    customRole_1 = new AppRole { Name = "HRManager", DisplayName = "İK Müdürü", Description = "İnsan kaynakları müdürü — tüm İK süreçlerine tam erişim, raporlama ve yetkilendirme", IsSystem = false, IsActive = true };
                    customRole_1.Id = _roleRepo.InsertAndGetId(customRole_1);
                    Console.WriteLine($"[Seed] Created custom role 'HRManager' (id={customRole_1.Id})");
                }
                var customPerms_1 = new[] {  };
                var customGranted_1 = _rolePermRepo.GetAll().Where(p => p.RoleId == customRole_1.Id).Select(p => p.PermissionName).ToList();
                var allPermSet_1 = new HashSet<string>(_permRegistry.All.Select(p => p.Name));
                foreach (var pn in customPerms_1.Except(customGranted_1))
                {
                    if (!allPermSet_1.Contains(pn)) { Console.WriteLine($"[Seed] Skipping unknown permission '{pn}' for role 'HRManager'"); continue; }
                    _rolePermRepo.Insert(new RolePermission { RoleId = customRole_1.Id, PermissionName = pn });
                }

                Console.WriteLine("[Seed] Step 7: Custom role 'DepartmentManager'");
                var customRole_2 = _roleRepo.GetAll().FirstOrDefault(r => r.Name == "DepartmentManager");
                if (customRole_2 == null)
                {
                    customRole_2 = new AppRole { Name = "DepartmentManager", DisplayName = "Departman Müdürü", Description = "Departman müdürü — kendi departmanındaki personelin izin, performans, fazla mesai ve disiplin süreçlerini yönetir", IsSystem = false, IsActive = true };
                    customRole_2.Id = _roleRepo.InsertAndGetId(customRole_2);
                    Console.WriteLine($"[Seed] Created custom role 'DepartmentManager' (id={customRole_2.Id})");
                }
                var customPerms_2 = new[] { "Employee.Read", "LeaveRequest.Read", "LeaveRequest.ChangeStatus", "PerformanceReview.Read", "PerformanceReview.Update", "PerformanceReview.ChangeStatus", "PerformanceGoal.Read", "PerformanceGoal.Update", "Training.Read", "TrainingParticipation.Read", "DisciplinaryRecord.Create", "DisciplinaryRecord.Read", "DisciplinaryRecord.Update", "OvertimeRecord.Read", "OvertimeRecord.ChangeStatus" };
                var customGranted_2 = _rolePermRepo.GetAll().Where(p => p.RoleId == customRole_2.Id).Select(p => p.PermissionName).ToList();
                var allPermSet_2 = new HashSet<string>(_permRegistry.All.Select(p => p.Name));
                foreach (var pn in customPerms_2.Except(customGranted_2))
                {
                    if (!allPermSet_2.Contains(pn)) { Console.WriteLine($"[Seed] Skipping unknown permission '{pn}' for role 'DepartmentManager'"); continue; }
                    _rolePermRepo.Insert(new RolePermission { RoleId = customRole_2.Id, PermissionName = pn });
                }

                Console.WriteLine("[Seed] Step 8: Custom role 'GeneralManager'");
                var customRole_3 = _roleRepo.GetAll().FirstOrDefault(r => r.Name == "GeneralManager");
                if (customRole_3 == null)
                {
                    customRole_3 = new AppRole { Name = "GeneralManager", DisplayName = "Genel Müdür", Description = "Genel müdür — tüm süreçlere okuma erişimi ve onay yetkisi", IsSystem = false, IsActive = true };
                    customRole_3.Id = _roleRepo.InsertAndGetId(customRole_3);
                    Console.WriteLine($"[Seed] Created custom role 'GeneralManager' (id={customRole_3.Id})");
                }
                var customPerms_3 = new[] {  };
                var customGranted_3 = _rolePermRepo.GetAll().Where(p => p.RoleId == customRole_3.Id).Select(p => p.PermissionName).ToList();
                var allPermSet_3 = new HashSet<string>(_permRegistry.All.Select(p => p.Name));
                foreach (var pn in customPerms_3.Except(customGranted_3))
                {
                    if (!allPermSet_3.Contains(pn)) { Console.WriteLine($"[Seed] Skipping unknown permission '{pn}' for role 'GeneralManager'"); continue; }
                    _rolePermRepo.Insert(new RolePermission { RoleId = customRole_3.Id, PermissionName = pn });
                }

                Console.WriteLine("[Seed] Step 9: Custom role 'Employee'");
                var customRole_4 = _roleRepo.GetAll().FirstOrDefault(r => r.Name == "Employee");
                if (customRole_4 == null)
                {
                    customRole_4 = new AppRole { Name = "Employee", DisplayName = "Personel", Description = "Standart personel — kendi sicil bilgilerini görür, izin talebi oluşturur, performans öz değerlendirmesi ve fazla mesai kaydı girer", IsSystem = false, IsActive = true };
                    customRole_4.Id = _roleRepo.InsertAndGetId(customRole_4);
                    Console.WriteLine($"[Seed] Created custom role 'Employee' (id={customRole_4.Id})");
                }
                var customPerms_4 = new[] { "Employee.Read", "LeaveRequest.Create", "LeaveRequest.Read", "PerformanceReview.Read", "PerformanceGoal.Create", "PerformanceGoal.Read", "PerformanceGoal.Update", "PeerReview.Create", "TrainingParticipation.Read", "EmployeeCertificate.Read", "OvertimeRecord.Create", "OvertimeRecord.Read", "DisciplinaryRecord.Read" };
                var customGranted_4 = _rolePermRepo.GetAll().Where(p => p.RoleId == customRole_4.Id).Select(p => p.PermissionName).ToList();
                var allPermSet_4 = new HashSet<string>(_permRegistry.All.Select(p => p.Name));
                foreach (var pn in customPerms_4.Except(customGranted_4))
                {
                    if (!allPermSet_4.Contains(pn)) { Console.WriteLine($"[Seed] Skipping unknown permission '{pn}' for role 'Employee'"); continue; }
                    _rolePermRepo.Insert(new RolePermission { RoleId = customRole_4.Id, PermissionName = pn });
                }
                Console.WriteLine("[Seed] DONE — login with admin / 123qwe");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Seed] FAILED: {ex.GetType().Name}: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[Seed] InnerException: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
                Console.WriteLine("[Seed] StackTrace:");
                Console.WriteLine(ex.StackTrace);
                throw; // bubble up to MigrationHostedService catch
            }
        }
    }
}
