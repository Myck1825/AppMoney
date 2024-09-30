using AppMoney.Options;

namespace AppMoney.HandleDockerSecrets
{
    public class SecretsConfigurationProvider(DockerSecrets dockerSecrets) : ConfigurationProvider
    {
        public override void Load()
        {
            if (Directory.Exists(dockerSecrets.Path))
            {
                var secretFiles = Directory.GetFiles(dockerSecrets.Path);
                foreach (var secretFile in secretFiles)
                {
                    var key = Path.GetFileName(secretFile);
                    var value = File.ReadAllText(secretFile).Trim();
                    Data.Add(key, value);
                }
            }
            //throw new InvalidOperationException("Could not load secrets");
        }
    }
}
