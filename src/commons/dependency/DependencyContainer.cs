public class DependencyContainer {

    public Optional<IRepository> Repository;
    public Optional<ServiceManagerEvents> Events;

    public DependencyContainer() {
        this.Repository = Optional<IRepository>.None();
        this.Events = Optional<ServiceManagerEvents>.None();
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

    public Optional<ServiceManagerEvents> GetEvents() {
        return this.Events;
    }

    public DependencyContainer SetEvents(ServiceManagerEvents events) {
        this.Events = Optional<ServiceManagerEvents>.Some(events);
        return this;
    }

}