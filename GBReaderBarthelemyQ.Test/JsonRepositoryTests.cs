using GBReaderBarthelemyQ.Domains;
using GBReaderBarthelemyQ.Repositories;
using GBReaderBarthelemyQ.Repositories.Exceptions;
using GBReaderBarthelemyQ.Repository.Json;
using NUnit.Framework;


namespace GBReaderBarthelemyQ.Test
{
    internal class JsonRepositoryTests
    {

        private string _fileName;

        private ISessionRepository _sessionRepository;

        [SetUp]
        public void Setup()
        {
            string runningPath = AppDomain.CurrentDomain.BaseDirectory;
            _fileName = Path.Combine(Path.GetFullPath(Path.Combine(runningPath, "..", "..", "..")), "Resources", "test.json");
            _sessionRepository = new SessionRepository(_fileName);
        }

        [TearDown]
        public void TearDown() => File.Delete(_fileName);

        [Test]
        public void NoExistingFile()
        {
            string runningPath = AppDomain.CurrentDomain.BaseDirectory;
            _fileName = Path.Combine(Path.GetFullPath(Path.Combine(runningPath, "..", "..", "..")), "Resources", "testNotExisting.json");
            
            _sessionRepository = new SessionRepository(_fileName);

            Assert.That(File.Exists(_fileName), Is.False);

            Assert.That(_sessionRepository.GetSessions(), Is.Empty);

            Assert.That(File.Exists(_fileName), Is.True);
        }

        [Test]
        public void BadFileFormat()
        {
            string runningPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = Path.Combine(Path.GetFullPath(Path.Combine(runningPath, "..", "..", "..")), "Resources", "badformat.json");

            _sessionRepository = new SessionRepository(fileName);

            Assert.That(_sessionRepository.GetSessions, Throws.InstanceOf<UnableToLoadException>());
        }

        [Test]
        public void MissingData()
        {
            string runningPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = Path.Combine(Path.GetFullPath(Path.Combine(runningPath, "..", "..", "..")), "Resources", "missingdata.json");

            _sessionRepository = new SessionRepository(fileName);

            Assert.That(_sessionRepository.GetSessions, Throws.InstanceOf<UnableToLoadException>());
        }

        [Test]
        public void BadDataFormat()
        {
            string runningPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = Path.Combine(Path.GetFullPath(Path.Combine(runningPath, "..", "..", "..")), "Resources", "baddataformat.json");

            _sessionRepository = new SessionRepository(fileName);

            Assert.That(() => _sessionRepository.GetSessions(), Throws.InstanceOf<UnableToLoadException>());
        }

        [Test]
        public void SaveSessions()
        {
            Dictionary<Isbn, Session> sessionsMap = new Dictionary<Isbn, Session>();

            Session session = new Session(DateTime.Parse("23-12-2022 15:30:14"), DateTime.Parse("23-12-2022 15:50:34"), 25);
            sessionsMap.Add(new Isbn("2-111111-00-7"), session);

            Session session1 = new Session(DateTime.Parse("24-12-2022 15:30:14"), DateTime.Parse("24-12-2022 15:50:34"), 20);
            sessionsMap.Add(new Isbn("2-111111-04-x"), session1);

            _sessionRepository.SaveBookSessions(sessionsMap);

            Assert.That(_sessionRepository.GetSessions().Count, Is.EqualTo(2));
        }

        [Test]
        public void Save0Session()
        {
            Dictionary<Isbn, Session> sessionsMap = new Dictionary<Isbn, Session>();

            _sessionRepository.SaveBookSessions(sessionsMap);

            Assert.That(_sessionRepository.GetSessions().Count, Is.EqualTo(0));
        }

        [Test]
        public void RetrieveSessions()
        {
            Dictionary<Isbn, Session> sessionsMap = new Dictionary<Isbn, Session>();

            Session session = new Session(DateTime.Parse("23-12-2022 15:30:14"), DateTime.Parse("23-12-2022 15:50:34"), 25);
            sessionsMap.Add(new Isbn("2-111111-00-7"), session);

            _sessionRepository.SaveBookSessions(sessionsMap);

            Assert.That(_sessionRepository.GetSessions().Count, Is.EqualTo(1));
            Assert.That(_sessionRepository.GetSessions().ContainsKey(new Isbn("2-111111-00-7")), Is.True);
            Assert.That(_sessionRepository.GetSessions()[new Isbn("2-111111-00-7")].Start, Is.EqualTo(DateTime.Parse("23-12-2022 15:30:14")));
            Assert.That(_sessionRepository.GetSessions()[new Isbn("2-111111-00-7")].Last, Is.EqualTo(DateTime.Parse("23-12-2022 15:50:34")));
            Assert.That(_sessionRepository.GetSessions()[new Isbn("2-111111-00-7")].LastPageNum, Is.EqualTo(25));
        }

    }
}
