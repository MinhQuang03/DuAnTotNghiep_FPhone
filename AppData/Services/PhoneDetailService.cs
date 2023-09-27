using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using AppData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Services
{
    public class PhoneDetailService : IPhoneDetailService
    {
        private IPhoneDetaildRepository _iPhoneDetaildRepository;
        private IPhoneRepository _iPhoneRepository;
        public PhoneDetailService(IPhoneDetaildRepository iPhoneDetaildRepository, IPhoneRepository phoneRepository)
        {
            _iPhoneDetaildRepository = iPhoneDetaildRepository;
            _iPhoneRepository = phoneRepository;
        }
        public async Task<List<PhoneDetaild>> GetPhoneDetailds()
        {
            var results = await _iPhoneDetaildRepository.GetAll();
            foreach (var phoneDetaild in results)
            {
                phoneDetaild.Phones = await _iPhoneRepository.GetById(phoneDetaild.IdPhone);
                // sau cần lấy thông tin khác thì add dependency repository vào rồi get tương tự
            }
            return results;
        }
    }
}
