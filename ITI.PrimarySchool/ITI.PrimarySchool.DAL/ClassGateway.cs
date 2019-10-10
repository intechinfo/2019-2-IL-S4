using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.PrimarySchool.DAL
{
    public class ClassGateway
    {
        public async Task<IEnumerable<ClassData>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ClassData
    {
        public int ClassId { get; set; }

        public string Name { get; set; }

        public string Level { get; set; }
    }
}
