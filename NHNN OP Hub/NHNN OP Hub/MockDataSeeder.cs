namespace NHNN_OP_Hub
{
    using NHNN_OP_Hub.Models;
    using System;
    using System.Collections.Generic;

    //This entire class was vide-coded

    public static class MockDataSeeder
    {
        public static List<PatientPackage> GetMockPackages()
        {
            return new List<PatientPackage>
        {
            // --- OUTPATIENT PACKAGES ---
            new OutpatientPackage
            {
                Name = "John Doe",
                MRN = "12345678", // Standard 8-digit
                DateDispensed = DateTime.Now.AddDays(-2),
                IsPaying = false,
                IsPrivate = false,
                RxCost = 0.00f,
                ReciptNumber = null,
                TicketNumber = 104,
                CollectionDate = DateOnly.FromDateTime(DateTime.Now),
                HasCollected = true,
                History = new PackageHistory() // Add your properties here if needed
            },
            new OutpatientPackage
            {
                Name = "Jane Smith",
                MRN = "87654321", // Standard 8-digit
                DateDispensed = DateTime.Now.AddDays(-5),
                IsPaying = true,
                IsPrivate = true,
                RxCost = 45.50f,
                ReciptNumber = 99283,
                TicketNumber = 208,
                CollectionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                HasCollected = false,
                History = new PackageHistory()
            },
            new OutpatientPackage
            {
                Name = "Alice Johnson",
                MRN = "A987654Z", // Alphanumeric test case
                DateDispensed = DateTime.Now.AddHours(-4),
                IsPaying = true,
                IsPrivate = false,
                RxCost = 9.35f,
                ReciptNumber = 44512,
                TicketNumber = null,
                CollectionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                HasCollected = false,
                History = new PackageHistory()
            },

            // --- POSTING PACKAGES ---
            new PostingPackage
            {
                Name = "Robert Brown",
                MRN = "11223344",
                DateDispensed = DateTime.Now.AddDays(-10),
                IsPaying = false,
                IsPrivate = false,
                RxCost = 0.00f,
                ReciptNumber = null,
                TrackingNumber = "GB1234567890",
                HasReturned = false,
                DeliveryAddress = "123 Fake Street, London, E1 411",
                History = new PackageHistory()
            },
            new PostingPackage
            {
                Name = "Emily Davis",
                MRN = "99887766",
                DateDispensed = DateTime.Now.AddDays(-15),
                IsPaying = true,
                IsPrivate = false,
                RxCost = 18.70f,
                ReciptNumber = 88211,
                TrackingNumber = "GB0987654321",
                HasReturned = true, // Testing a returned package
                DeliveryAddress = "456 Mockingbird Lane, Manchester, M1 2AB",
                History = new PackageHistory()
            },
            new PostingPackage
            {
                Name = "Michael Wilson",
                MRN = "M1234-67", // Alphanumeric with symbol test case
                DateDispensed = DateTime.Now.AddDays(-1),
                IsPaying = true,
                IsPrivate = true,
                RxCost = 120.00f,
                ReciptNumber = 33219,
                TrackingNumber = null, // Not yet shipped
                HasReturned = false,
                DeliveryAddress = "789 Test Avenue, Birmingham, B1 1BB",
                History = new PackageHistory()
            }
        };
        }
    }
}
