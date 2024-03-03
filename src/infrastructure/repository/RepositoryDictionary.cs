public class RepositoryDictionary {

    public static Optional<IRepository> Find(Persistence persistence) {
        return persistence.code switch {
            RepositoryMySQL.NAME => Optional<IRepository>.Some(new RepositoryMySQL(persistence)),
            _ => Optional<IRepository>.None(),
        };
    }

}