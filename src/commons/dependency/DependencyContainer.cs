public class DependencyContainer {

    public Optional<IRepository> Repository;

    public DependencyContainer() {
        this.Repository = Optional<IRepository>.None();
    }

    public Optional<IRepository> GetRepository() {
        return this.Repository;
    }

    public DependencyContainer SetRepository(IRepository repository) {
        this.Repository = Optional<IRepository>.Some(repository);
        return this;
    }

     public DependencyContainer SetRepository(Optional<IRepository> repository) {
        this.Repository = repository;
        return this;
    }

}