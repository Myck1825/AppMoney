using AppMoney.Options;

namespace AppMoney.HandleDockerSecrets
{
    public class SecretsConfigurationSource(DockerSecrets dockerSecrets) : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SecretsConfigurationProvider(dockerSecrets);
        }
    }
}
