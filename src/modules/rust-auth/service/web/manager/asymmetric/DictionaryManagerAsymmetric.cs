internal class DictionaryManagerAsymmetric {

    internal static Optional<IManagerAsymmetric> Find(string code) {
        switch (code) {
            case ManagerRsa.NAME:
                return Optional<IManagerAsymmetric>.Some(new ManagerRsa());
            default:
                return Optional<IManagerAsymmetric>.None();
        }
    }

}