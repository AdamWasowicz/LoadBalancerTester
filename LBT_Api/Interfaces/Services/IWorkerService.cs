using LBT_Api.Models.WorkerDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IWorkerService
    {
        GetWorkerDto Create(CreateWorkerDto dto);
        public int Delete(int id);
        public GetWorkerDto Read(int id);
        public GetWorkerDto[] ReadAll();
        public GetWorkerDto Update(UpdateWorkerDto dto);
    }
}
