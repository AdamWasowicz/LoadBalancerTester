using LBT_Api.Models.WorkerDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IWorkerService
    {
        public GetWorkerDto Create(CreateWorkerDto dto);
        public GetWorkerWithDependenciesDto CreateWithDependencies(CreateWorkerWithDependenciesDto dto);
        public void Delete(int id);
        public GetWorkerDto Read(int id);
        public GetWorkerWithDependenciesDto ReadWithDependencies(int id);
        public GetWorkerDto[] ReadAll();
        public GetWorkerWithDependenciesDto[] ReadAllWithDependencies();
        public GetWorkerDto Update(UpdateWorkerDto dto);

        // Seed
        public void CreateExampleData(int amount);
    }
}
