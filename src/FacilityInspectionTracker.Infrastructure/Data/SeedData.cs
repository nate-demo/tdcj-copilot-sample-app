using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Infrastructure.Data;

public static class SeedData
{
    public static List<Facility> GetFacilities()
    {
        return new List<Facility>
        {
            new Facility
            {
                Id = 1,
                Name = "Huntsville Unit",
                Address = "815 12th St",
                City = "Huntsville",
                State = "TX",
                ZipCode = "77340",
                FacilityType = "Maximum Security",
                Capacity = 1600,
                IsActive = true,
                CreatedAt = new DateTime(1849, 1, 1)
            },
            new Facility
            {
                Id = 2,
                Name = "Darrington Unit",
                Address = "59 Darrington Rd",
                City = "Rosharon",
                State = "TX",
                ZipCode = "77583",
                FacilityType = "Maximum Security",
                Capacity = 1930,
                IsActive = true,
                CreatedAt = new DateTime(1917, 3, 15)
            },
            new Facility
            {
                Id = 3,
                Name = "Goree Unit",
                Address = "7317 Clemens Rd",
                City = "Huntsville",
                State = "TX",
                ZipCode = "77340",
                FacilityType = "Medium Security",
                Capacity = 580,
                IsActive = true,
                CreatedAt = new DateTime(1907, 6, 1)
            },
            new Facility
            {
                Id = 4,
                Name = "Eastham Unit",
                Address = "2665 Prison Rd 1",
                City = "Lovelady",
                State = "TX",
                ZipCode = "75851",
                FacilityType = "Maximum Security",
                Capacity = 2400,
                IsActive = true,
                CreatedAt = new DateTime(1917, 1, 1)
            },
            new Facility
            {
                Id = 5,
                Name = "Wynne Unit",
                Address = "810 FM 2821",
                City = "Huntsville",
                State = "TX",
                ZipCode = "77349",
                FacilityType = "Medium Security",
                Capacity = 2630,
                IsActive = true,
                CreatedAt = new DateTime(1879, 4, 20)
            }
        };
    }

    public static List<Inspection> GetInspections()
    {
        return new List<Inspection>
        {
            new Inspection
            {
                Id = 1,
                FacilityId = 1,
                InspectorName = "James Holden",
                InspectionType = "Fire Safety",
                ScheduledDate = DateTime.Now.AddDays(-30),
                CompletedDate = DateTime.Now.AddDays(-29),
                Status = InspectionStatus.Completed,
                Priority = InspectionPriority.High,
                Notes = "All fire suppression systems operational. Exit signage updated.",
                Score = 87,
                CreatedAt = DateTime.Now.AddDays(-35)
            },
            new Inspection
            {
                Id = 2,
                FacilityId = 1,
                InspectorName = "Maria Santos",
                InspectionType = "Health & Sanitation",
                ScheduledDate = DateTime.Now.AddDays(7),
                Status = InspectionStatus.Scheduled,
                Priority = InspectionPriority.Medium,
                Notes = "Routine quarterly sanitation review.",
                CreatedAt = DateTime.Now.AddDays(-3)
            },
            new Inspection
            {
                Id = 3,
                FacilityId = 2,
                InspectorName = "Robert Chen",
                InspectionType = "Structural Integrity",
                ScheduledDate = DateTime.Now.AddDays(-60),
                CompletedDate = DateTime.Now.AddDays(-58),
                Status = InspectionStatus.Completed,
                Priority = InspectionPriority.Critical,
                Notes = "North wing foundation shows minor cracking. Follow-up required.",
                Score = 72,
                CreatedAt = DateTime.Now.AddDays(-65)
            },
            new Inspection
            {
                Id = 4,
                FacilityId = 3,
                InspectorName = "Linda Okafor",
                InspectionType = "Security Systems",
                ScheduledDate = DateTime.Now.AddDays(14),
                Status = InspectionStatus.Scheduled,
                Priority = InspectionPriority.High,
                Notes = "Annual security camera and perimeter review.",
                CreatedAt = DateTime.Now.AddDays(-1)
            },
            new Inspection
            {
                Id = 5,
                FacilityId = 4,
                InspectorName = "Thomas Wright",
                InspectionType = "Plumbing & Water Systems",
                ScheduledDate = DateTime.Now.AddDays(-15),
                Status = InspectionStatus.Cancelled,
                Priority = InspectionPriority.Low,
                Notes = "Cancelled due to inspector illness. Rescheduling pending.",
                CreatedAt = DateTime.Now.AddDays(-20)
            },
            new Inspection
            {
                Id = 6,
                FacilityId = 5,
                InspectorName = "Emily Nguyen",
                InspectionType = "Electrical Systems",
                ScheduledDate = DateTime.Now.AddDays(3),
                Status = InspectionStatus.Scheduled,
                Priority = InspectionPriority.Medium,
                Notes = "Panel upgrades scheduled for next month. Pre-inspection required.",
                CreatedAt = DateTime.Now.AddDays(-5)
            }
        };
    }
}
