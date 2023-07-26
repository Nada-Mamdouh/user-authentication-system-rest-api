using Microsoft.EntityFrameworkCore;
using UserAuthenticationSystem.Models;

namespace UserAuthenticationSystem.Repositories
{
    public class HashAlgoRepo : IHashAlgoRepo
    {
        UserAuthenticationSystemDbContext _dbContext;
        public HashAlgoRepo(UserAuthenticationSystemDbContext dbContext) {
            _dbContext = dbContext;
        }

        public int AddHashAlgo(string algorithmname)
        {
            HashingAlgorithm ha = GetAlgorithmByName(algorithmname);
            if (ha == null)
            {
                try
                {
                    ha = new HashingAlgorithm();
                    ha.AlgorithmName = algorithmname;
                    _dbContext.Add(ha);
                    _dbContext.SaveChanges();
                    return ha.HashAlgorithmId;
                }
                catch (Exception e)
                {
                    throw new Exception($"exeption while saving: {e.Message}");
                }
            }
            return ha.HashAlgorithmId;
        }

        public HashingAlgorithm GetAlgorithmByName(string algoname)
        {
            HashingAlgorithm ha = _dbContext.HashingAlgorithms.FirstOrDefault(hashalgo => hashalgo.AlgorithmName == algoname);
            return ha;
        }
    }
}
