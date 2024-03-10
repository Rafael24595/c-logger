internal class DictionaryManagerSymmetric {

    internal static Optional<IManagerSymmetric> Find(string code, IConfigurationSection args) {
        switch (code) {
            case ManagerAesGcm.NAME:
                int size = args.GetValue<int>("size");
                return  Optional<IManagerSymmetric>.Some(new ManagerAesGcm(size));
            default:
                return Optional<IManagerSymmetric>.None();
        }
    }

}