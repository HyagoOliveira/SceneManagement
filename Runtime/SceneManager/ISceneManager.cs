using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    public interface ISceneManager
    {
        Task LoadScene(string scene);
    }
}