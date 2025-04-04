using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository;

namespace BusinessLogicLayer.Services
{
    public class PregnancyRecordService
    {
        private readonly PregnancyRecordRepository _repository;

        public PregnancyRecordService()
        {
            _repository = new PregnancyRecordRepository();
        }

        // Lấy tất cả các hồ sơ mang thai
        public List<PregnancyRecord> GetAllPregnancyRecords(Guid userId)
        {
            return _repository.GetAllRecords()
                .Where(r => r.UserId == userId)
                .ToList();
        }

        // Lấy hồ sơ mang thai theo ID
        public PregnancyRecord? GetPregnancyRecordById(Guid id)
        {
            return _repository.GetRecordById(id);
        }

        // Thêm mới hồ sơ mang thai
        public bool AddPregnancyRecord(PregnancyRecord record)
        {
            if (record == null) return false;
            return _repository.AddRecord(record);
        }

        // Cập nhật hồ sơ mang thai
        public bool UpdatePregnancyRecord(PregnancyRecord record)
        {
            if (record == null) return false;
            return _repository.UpdateRecord(record);
        }

        // Xóa hồ sơ mang thai
        public bool DeletePregnancyRecord(Guid id)
        {
            return _repository.DeleteRecord(id);
        }

        // Tìm kiếm hồ sơ mang thai theo tên bé hoặc giới tính
        public List<PregnancyRecord> SearchPregnancyRecords(string searchTerm)
        {
            return _repository.SearchRecords(searchTerm);
        }
    }
}
