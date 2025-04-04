using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Repository
{
    public class PregnancyRecordRepository
    {
        private readonly PregnaCareAppDbContext _context;

        public PregnancyRecordRepository()
        {
            _context = new PregnaCareAppDbContext();
        }

        public List<PregnancyRecord> GetAllRecords()
        {
            return _context.PregnancyRecords
                .Where(r => r.IsDeleted == false)
                .ToList();
        }

        public PregnancyRecord? GetRecordById(Guid id)
        {
            return _context.PregnancyRecords.FirstOrDefault(r => r.Id == id && r.IsDeleted == false);
        }

        public bool AddRecord(PregnancyRecord record)
        {
            if (record == null) return false;

            record.Id = Guid.NewGuid();
            record.CreatedAt = DateTime.Now;
            record.UpdatedAt = DateTime.Now;
            record.IsDeleted = false;

            _context.PregnancyRecords.Add(record);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateRecord(PregnancyRecord record)
        {
            var existingRecord = _context.PregnancyRecords.FirstOrDefault(r => r.Id == record.Id && r.IsDeleted == false);
            if (existingRecord == null) return false;

            existingRecord.BabyName = record.BabyName;
            existingRecord.PregnancyStartDate = record.PregnancyStartDate;
            existingRecord.ExpectedDueDate = record.ExpectedDueDate;
            existingRecord.BabyGender = record.BabyGender;
            existingRecord.ImageUrl = record.ImageUrl;
            existingRecord.UpdatedAt = DateTime.Now;

            return _context.SaveChanges() > 0;
        }

        public bool DeleteRecord(Guid id)
        {
            var record = _context.PregnancyRecords.FirstOrDefault(r => r.Id == id);
            if (record == null) return false;

            record.IsDeleted = true;
            return _context.SaveChanges() > 0;
        }

        public List<PregnancyRecord> SearchRecords(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAllRecords();

            searchTerm = searchTerm.ToLower();

            return _context.PregnancyRecords
                .Where(r => r.IsDeleted == false &&
                            (r.BabyName.ToLower().Contains(searchTerm) ||
                             r.BabyGender.ToLower().Contains(searchTerm)))
                .ToList();
        }
    }
}
