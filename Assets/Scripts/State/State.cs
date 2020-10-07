// ReSharper disable All
namespace State {
	public interface State {
		void Enter(Player player);
		void Resume(Player player);
		void Exit(Player player);	
	}
}