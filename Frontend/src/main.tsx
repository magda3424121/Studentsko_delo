import React from 'react';
import { createRoot } from 'react-dom/client';
import './styles.css';

type Job = {
  id: number;
  ime: string;
  opis: string;
  placa: number;
  delovnik: string;
  trajanje: string;
  podjetja_id: number;
  lokacija?: string;
  kraj_id?: number;
};

type Company = {
  id: number;
  ime: string;
  opis?: string;
  mail: string;
  telefon: string;
  spletna_stran?: string;
  geslo: string;
  kraji_id: number;
};

type User = {
  id: number;
  ime: string;
  priimek: string;
  telefon: string;
  mail: string;
  sola?: string;
  geslo: string;
  cv: string;
  vrste_uporabnikov_id: number;
  kraji_id: number;
};

type Place = {
  id: number;
  ime: string;
  postna_st: number;
};

type Application = {
  id: number;
  datum_prijave: string;
  status: string;
  objave_id: number;
  naziv_dela: string;
  lokacija?: string;
  placa: number;
  uporabniki_id?: number;
  ime?: string;
  priimek?: string;
  mail?: string;
  telefon?: string;
  sola?: string;
  podjetje?: string;
};

type EntityConfig = {
  label: string;
  endpoint: string;
  createEndpoint?: string;
  fields: Array<[string, string, 'text' | 'email' | 'password' | 'number' | 'textarea' | 'datetime-local']>;
};

const pageAliases: Record<string, string> = {
  '/': 'home',
  '/index.html': 'home',
  '/prijava.html': 'login-choice',
  '/registracija-student.html': 'student-auth',
  '/prijava-podjetje.html': 'company-login',
  '/registracija-podjetje.html': 'company-register',
  '/podjetja-index.html': 'company-dashboard',
  '/admin-prijava.html': 'admin-login',
  '/admin.html': 'admin',
  '/za-studente.html': 'students-info',
  '/za-podjetje.html': 'company-info'
};

const api = {
  async get<T>(url: string): Promise<T> {
    const response = await fetch(url);
    if (!response.ok) throw new Error(await response.text());
    return response.json() as Promise<T>;
  },
  async send<T>(url: string, method: string, data: unknown): Promise<T> {
    const response = await fetch(url, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });
    if (!response.ok) throw new Error(await response.text());
    return response.status === 204 ? ({} as T) : (response.json() as Promise<T>);
  }
};

function getPage(): string {
  const queryPage = new URLSearchParams(window.location.search).get('page');
  return queryPage || pageAliases[window.location.pathname] || 'home';
}

function go(path: string) {
  window.location.href = path;
}

function App() {
  const [page] = React.useState(getPage());

  if (page === 'login-choice') return <LoginChoice />;
  if (page === 'student-auth') return <StudentAuth />;
  if (page === 'company-login') return <CompanyLogin />;
  if (page === 'company-register') return <CompanyRegister />;
  if (page === 'company-dashboard') return <CompanyDashboard />;
  if (page === 'admin-login') return <AdminLogin />;
  if (page === 'admin') return <AdminPage />;
  if (page === 'students-info') return <StudentsInfo />;
  if (page === 'company-info') return <CompanyInfo />;
  return <Home />;
}

function Home() {
  const [jobs, setJobs] = React.useState<Job[]>([]);
  const [selectedJob, setSelectedJob] = React.useState<Job | null>(null);
  const [companyName, setCompanyName] = React.useState('');
  const [message, setMessage] = React.useState('');
  const [myApplications, setMyApplications] = React.useState<Application[] | null>(null);
  const [search, setSearch] = React.useState('');
  const [region, setRegion] = React.useState('');
  const [type, setType] = React.useState('');
  const [minPay, setMinPay] = React.useState(0);

  const userName = localStorage.getItem('user_name');
  const userRole = localStorage.getItem('user_role');

  React.useEffect(() => {
    void loadJobs();
  }, []);

  async function loadJobs() {
    setJobs(await api.get<Job[]>('/api/JobOffer'));
  }

  async function openDetails(job: Job) {
    setSelectedJob(job);
    setMessage('');
    setMyApplications(null);
    try {
      const company = await api.get<Company>(`/api/Podjetje/${job.podjetja_id}`);
      setCompanyName(company.ime);
    } catch {
      setCompanyName(`ID podjetja: ${job.podjetja_id}`);
    }
  }

  async function applyForJob() {
    const userId = localStorage.getItem('user_id');
    if (!selectedJob) return;
    if (!userId || userRole !== '1') {
      setMessage('Za prijavo na delo se morate najprej prijaviti kot dijak/študent.');
      return;
    }

    try {
      await api.send('/api/Prijava', 'POST', {
        uporabniki_id: Number(userId),
        objave_id: selectedJob.id,
        status: 'V obravnavi'
      });
      setMessage('Prijava je bila poslana delodajalcu. Status je: V obravnavi.');
    } catch (error) {
      const text = String(error);
      setMessage(text.includes('že prijavili') ? 'Na ta oglas ste že prijavljeni.' : 'Prijave trenutno ni mogoče poslati.');
    }
  }

  async function showMyApplications() {
    const userId = localStorage.getItem('user_id');
    if (!userId) return;
    setSelectedJob(null);
    setMyApplications(await api.get<Application[]>(`/api/Prijava/uporabnik/${userId}`));
  }

  function logout() {
    localStorage.clear();
    window.location.reload();
  }

  const filteredJobs = jobs.filter((job) => {
    const q = search.toLowerCase();
    const jobText = `${job.ime} ${job.opis}`.toLowerCase();
    const location = (job.lokacija || '').toLowerCase();
    const typeMatch = type === '' || jobText.includes(type);
    return jobText.includes(q) && (region === '' || location.includes(region)) && typeMatch && (job.placa || 0) >= minPay;
  });

  return (
    <>
      <div className="top-bar">
        <div className="container-wide">
          <span>Študentska dela, prijave in delodajalci na enem mestu</span>
          <div className="auth-section">
            {userName ? (
              <>
                <span className="user-name">{userName}</span>
                {userRole === '1' && <button className="btn-small" onClick={showMyApplications}>Moje prijave</button>}
                <button className="text-button danger" onClick={logout}>Odjava</button>
              </>
            ) : (
              <button className="btn-login" onClick={() => go('/prijava.html')}>Prijava / Registracija</button>
            )}
          </div>
        </div>
      </div>

      <Header />

      <section className="search-section">
        <h1>Odkrij svojo naslednjo priložnost</h1>
        <div className="search-bar">
          <input value={search} onChange={(event) => setSearch(event.target.value)} placeholder="Išči po nazivu, kraju..." />
          <button onClick={loadJobs}>Išči</button>
        </div>
        <div className="filters">
          <select value={region} onChange={(event) => setRegion(event.target.value)}>
            <option value="">Vse regije</option>
            <option value="ljubljana">Ljubljana</option>
            <option value="maribor">Maribor</option>
            <option value="obala">Obala</option>
            <option value="celje">Celje</option>
          </select>
          <select value={type} onChange={(event) => setType(event.target.value)}>
            <option value="">Vse vrste dela</option>
            <option value="prodaja">Prodaja</option>
            <option value="it">IT</option>
            <option value="gostinstvo">Gostinstvo</option>
            <option value="marketing">Marketing</option>
          </select>
          <select value={minPay} onChange={(event) => setMinPay(Number(event.target.value))}>
            <option value={0}>Min. plačilo</option>
            <option value={7}>Nad 7 €/h</option>
            <option value={9}>Nad 9 €/h</option>
            <option value={11}>Nad 11 €/h</option>
          </select>
        </div>
      </section>

      <main className="job-container" id="job-container">
        {filteredJobs.length === 0 ? <p className="center">Ni najdenih del za izbrane filtre.</p> : filteredJobs.map((job) => (
          <article className="job-card" key={job.id}>
            <div className="job-head">
              <div>
                <h2>{job.ime}</h2>
                <div className="job-location">{job.lokacija?.toUpperCase() || 'LOKACIJA PO DOGOVORU'}</div>
              </div>
            </div>
            <div className="pay-row">
              <span>{(job.placa || 0).toFixed(2)} €/h neto</span>
              <small>({((job.placa || 0) * 1.15).toFixed(2)} €/h bruto)</small>
            </div>
            <p>{job.opis}</p>
            <footer className="job-footer">
              <span>Trajanje: <b>{job.trajanje || 'Po dogovoru'}</b> • Delovnik: <b>{job.delovnik || 'Po dogovoru'}</b></span>
              <button className="btn-more" onClick={() => void openDetails(job)}>Prikaži podrobnosti</button>
            </footer>
          </article>
        ))}
      </main>

      {(selectedJob || myApplications) && (
        <Modal title={selectedJob?.ime || 'Moje prijave'} onClose={() => { setSelectedJob(null); setMyApplications(null); }}>
          {selectedJob && (
            <>
              <p>{selectedJob.opis || 'Opis ni vpisan.'}</p>
              <div className="detail-grid">
                <Info label="Podjetje" value={companyName} />
                <Info label="Lokacija" value={selectedJob.lokacija || 'Po dogovoru'} />
                <Info label="Plačilo" value={`${(selectedJob.placa || 0).toFixed(2)} €/h neto`} />
                <Info label="Delovnik" value={selectedJob.delovnik || 'Po dogovoru'} />
                <Info label="Trajanje" value={selectedJob.trajanje || 'Po dogovoru'} />
                <Info label="Šifra oglasa" value={String(selectedJob.id)} />
              </div>
              <button className="btn-primary" onClick={() => void applyForJob()}>Prijavi se na delo</button>
              {message && <p className="modal-message">{message}</p>}
            </>
          )}
          {myApplications && (
            myApplications.length === 0 ? <p>Niste še oddali nobene prijave.</p> : myApplications.map((application) => (
              <div className="application-row" key={application.id}>
                <b>{application.naziv_dela}</b><br />
                <span>{application.podjetje} • {application.lokacija || 'Lokacija po dogovoru'} • {(application.placa || 0).toFixed(2)} €/h</span><br />
                <span className="status-pill">{application.status || 'V obravnavi'}</span>
              </div>
            ))
          )}
        </Modal>
      )}
    </>
  );
}

function Header() {
  return (
    <header className="main-header">
      <div className="container-wide main-nav">
        <button className="logo-button" onClick={() => go('/index.html')}><img src="/img/logo.png" alt="Logo" /></button>
        <nav>
          <button onClick={() => go('/index.html#job-container')}>Iskalnik del</button>
          <button onClick={() => go('/za-studente.html')}>Blog</button>
          <button onClick={() => go('/admin-prijava.html')}>Admin</button>
        </nav>
        <div className="action-buttons">
          <button className="btn-student" onClick={() => go('/za-studente.html')}>Za študente</button>
          <button className="btn-company" onClick={() => go('/za-podjetje.html')}>Za podjetja</button>
        </div>
      </div>
    </header>
  );
}

function LoginChoice() {
  return (
    <AuthShell>
      <h1>Pozdravljeni v portalu!</h1>
      <p>Za nadaljevanje izberite vrsto računa:</p>
      <div className="selection-cards">
        <ChoiceCard title="Sem dijak / študent" icon="🎓" text="Iščem delo in želim oddati prijavo na oglas." path="/registracija-student.html" button="Prijava / Registracija" />
        <ChoiceCard title="Sem podjetje" icon="🏢" text="Objavljam prosta dela in urejam prijave kandidatov." path="/prijava-podjetje.html" button="Prijava / Registracija" />
        <ChoiceCard title="Sem administrator" icon="🔐" text="Upravljam podatke, uporabnike, podjetja in prijave." path="/admin-prijava.html" button="Admin prijava" />
      </div>
    </AuthShell>
  );
}

function ChoiceCard(props: { title: string; icon: string; text: string; path: string; button: string }) {
  return (
    <button className="choice-card" onClick={() => go(props.path)}>
      <span className="choice-icon">{props.icon}</span>
      <h2>{props.title}</h2>
      <p>{props.text}</p>
      <span>{props.button}</span>
    </button>
  );
}

function StudentAuth() {
  const [isRegistering, setIsRegistering] = React.useState(false);
  const [form, setForm] = React.useState({ ime: '', priimek: '', telefon: '', mail: '', sola: '', geslo: '' });

  async function login() {
    const user = await api.send<{ id: number; ime: string; priimek: string; mail: string; vrste_uporabnikov_id: number }>('/api/Auth/login', 'POST', {
      mail: form.mail,
      geslo: form.geslo
    });
    localStorage.setItem('user_id', String(user.id));
    localStorage.setItem('user_name', `${user.ime} ${user.priimek}`);
    localStorage.setItem('user_role', '1');
    localStorage.setItem('is_logged_in', 'true');
    go('/index.html');
  }

  async function register() {
    await api.send('/api/Auth/register', 'POST', {
      ...form,
      cv: '',
      vrste_uporabnikov_id: 1,
      kraji_id: 1
    });
    setIsRegistering(false);
    alert('Registracija uspešna. Zdaj se prijavite.');
  }

  return (
    <AuthShell>
      <div className="auth-card">
        <h1>{isRegistering ? 'Registracija študenta' : 'Prijava študenta'}</h1>
        {isRegistering && (
          <div className="two-cols">
            <TextInput label="Ime" value={form.ime} onChange={(ime) => setForm({ ...form, ime })} />
            <TextInput label="Priimek" value={form.priimek} onChange={(priimek) => setForm({ ...form, priimek })} />
          </div>
        )}
        {isRegistering && <TextInput label="Telefon" value={form.telefon} onChange={(telefon) => setForm({ ...form, telefon })} />}
        <TextInput label="E-pošta" value={form.mail} onChange={(mail) => setForm({ ...form, mail })} />
        {isRegistering && <TextInput label="Šola / Fakulteta" value={form.sola} onChange={(sola) => setForm({ ...form, sola })} />}
        <TextInput label="Geslo" type="password" value={form.geslo} onChange={(geslo) => setForm({ ...form, geslo })} />
        <button className="btn-primary wide" onClick={() => void (isRegistering ? register() : login())}>{isRegistering ? 'Ustvari račun' : 'Vpiši se'}</button>
        <button className="text-button" onClick={() => setIsRegistering(!isRegistering)}>{isRegistering ? 'Že imate račun? Prijava' : 'Nimate računa? Registracija'}</button>
      </div>
    </AuthShell>
  );
}

function CompanyLogin() {
  const [mail, setMail] = React.useState('');
  const [geslo, setGeslo] = React.useState('');
  const [error, setError] = React.useState('');

  async function login() {
    try {
      const company = await api.send<{ id: number; ime: string; mail: string }>('/api/Podjetje/login', 'POST', { mail, geslo });
      localStorage.setItem('podjetje_id', String(company.id));
      localStorage.setItem('user_name', company.ime || 'Podjetje');
      localStorage.setItem('user_role', '2');
      localStorage.setItem('is_logged_in', 'true');
      go('/podjetja-index.html');
    } catch {
      setError('Napačna e-pošta ali geslo.');
    }
  }

  return (
    <AuthShell>
      <div className="auth-card">
        <h1>Prijava podjetja</h1>
        <TextInput label="E-pošta" value={mail} onChange={setMail} />
        <TextInput label="Geslo" type="password" value={geslo} onChange={setGeslo} />
        <button className="btn-primary wide" onClick={() => void login()}>Vpiši se</button>
        {error && <p className="error">{error}</p>}
        <button className="text-button" onClick={() => go('/registracija-podjetje.html')}>Registracija podjetja</button>
      </div>
    </AuthShell>
  );
}

function CompanyRegister() {
  const [places, setPlaces] = React.useState<Place[]>([]);
  const [form, setForm] = React.useState({ ime: '', mail: '', telefon: '', spletna_stran: '', opis: '', geslo: '', kraji_id: '' });

  React.useEffect(() => {
    api.get<Place[]>('/api/Podjetje/kraji').then(setPlaces).catch(() => setPlaces([]));
  }, []);

  async function register() {
    await api.send('/api/Podjetje/register', 'POST', { ...form, kraji_id: Number(form.kraji_id) });
    alert('Podjetje uspešno registrirano.');
    go('/prijava-podjetje.html');
  }

  return (
    <AuthShell>
      <div className="auth-card large">
        <h1>Registracija podjetja</h1>
        <TextInput label="Ime podjetja" value={form.ime} onChange={(ime) => setForm({ ...form, ime })} />
        <div className="two-cols">
          <TextInput label="E-pošta" value={form.mail} onChange={(mail) => setForm({ ...form, mail })} />
          <TextInput label="Telefon" value={form.telefon} onChange={(telefon) => setForm({ ...form, telefon })} />
        </div>
        <label>Kraj podjetja
          <select value={form.kraji_id} onChange={(event) => setForm({ ...form, kraji_id: event.target.value })}>
            <option value="">Izberi kraj</option>
            {places.map((place) => <option value={place.id} key={place.id}>{place.ime}</option>)}
          </select>
        </label>
        <TextInput label="Spletna stran" value={form.spletna_stran} onChange={(spletna_stran) => setForm({ ...form, spletna_stran })} />
        <label>Opis dejavnosti<textarea value={form.opis} onChange={(event) => setForm({ ...form, opis: event.target.value })} /></label>
        <TextInput label="Geslo" type="password" value={form.geslo} onChange={(geslo) => setForm({ ...form, geslo })} />
        <button className="btn-primary wide" onClick={() => void register()}>Ustvari račun podjetja</button>
      </div>
    </AuthShell>
  );
}

function CompanyDashboard() {
  const companyId = Number(localStorage.getItem('podjetje_id'));
  const role = localStorage.getItem('user_role');
  const name = localStorage.getItem('user_name') || 'Podjetje';
  const [jobs, setJobs] = React.useState<Job[]>([]);
  const [applications, setApplications] = React.useState<Application[]>([]);
  const [editing, setEditing] = React.useState<Job | null>(null);
  const [form, setForm] = React.useState({ ime: '', placa: '', lokacija: '', trajanje: '', opis: '' });

  React.useEffect(() => {
    if (role !== '2' || !companyId) go('/prijava-podjetje.html');
    void refresh();
  }, []);

  async function refresh() {
    const [allJobs, allApplications] = await Promise.all([
      api.get<Job[]>('/api/JobOffer'),
      api.get<Application[]>(`/api/Prijava/podjetje/${companyId}`)
    ]);
    setApplications(allApplications);
    setJobs(allJobs.filter((job) => job.podjetja_id === companyId));
  }

  async function saveJob() {
    const data = {
      id: editing?.id || 0,
      ime: form.ime,
      opis: form.opis,
      placa: Number(form.placa),
      lokacija: form.lokacija,
      trajanje: form.trajanje,
      delovnik: 'Po dogovoru',
      podjetja_id: companyId,
      kraj_id: 1
    };
    await api.send(editing ? `/api/JobOffer/${editing.id}` : '/api/JobOffer', editing ? 'PUT' : 'POST', data);
    setEditing(null);
    setForm({ ime: '', placa: '', lokacija: '', trajanje: '', opis: '' });
    await refresh();
  }

  function startEdit(job: Job) {
    setEditing(job);
    setForm({ ime: job.ime, placa: String(job.placa), lokacija: job.lokacija || '', trajanje: job.trajanje || '', opis: job.opis || '' });
  }

  async function deleteJob(id: number) {
    if (!confirm('Ali ste prepričani?')) return;
    await api.send(`/api/JobOffer/${id}`, 'DELETE', {});
    await refresh();
  }

  async function updateStatus(id: number, status: string) {
    await api.send(`/api/Prijava/${id}/status`, 'PUT', { status });
    await refresh();
  }

  return (
    <DashboardShell title={`Pozdravljeni, ${name}!`}>
      <section className="panel">
        <h2>Vaši aktivni oglasi</h2>
        <DataTable headers={['Naziv dela', 'Plačilo', 'Lokacija', 'Prijave', 'Akcije']}>
          {jobs.map((job) => (
            <tr key={job.id}>
              <td><b>{job.ime}</b></td>
              <td>{(job.placa || 0).toFixed(2)} €/h</td>
              <td>{job.lokacija || 'Slovenija'}</td>
              <td>{applications.filter((application) => application.objave_id === job.id).length}</td>
              <td><button onClick={() => startEdit(job)}>Uredi</button> <button className="danger-btn" onClick={() => void deleteJob(job.id)}>Odstrani</button></td>
            </tr>
          ))}
        </DataTable>
      </section>

      <section className="panel">
        <h2>Prijave dijakov in študentov</h2>
        <DataTable headers={['Delo', 'Kandidat', 'Podatki kandidata', 'Datum', 'Status', 'Akcije']}>
          {applications.map((application) => (
            <tr key={application.id}>
              <td><b>{application.naziv_dela}</b><br /><small>{application.lokacija || 'Lokacija po dogovoru'}</small></td>
              <td>{application.ime} {application.priimek}<br /><small>{application.sola}</small></td>
              <td>{application.mail}<br />{application.telefon}</td>
              <td>{new Date(application.datum_prijave).toLocaleString('sl-SI')}</td>
              <td><span className={`status-pill ${application.status === 'Sprejeta' ? 'accepted' : application.status === 'Zavrnjena' ? 'rejected' : ''}`}>{application.status}</span></td>
              <td><button onClick={() => void updateStatus(application.id, 'Sprejeta')}>Sprejmi</button> <button className="danger-btn" onClick={() => void updateStatus(application.id, 'Zavrnjena')}>Zavrni</button></td>
            </tr>
          ))}
        </DataTable>
      </section>

      <section className="panel form-panel">
        <h2>{editing ? 'Uredi prosto delo' : 'Objavi novo prosto delo'}</h2>
        <div className="two-cols">
          <TextInput label="Naziv dela" value={form.ime} onChange={(ime) => setForm({ ...form, ime })} />
          <TextInput label="Urna postavka" type="number" value={form.placa} onChange={(placa) => setForm({ ...form, placa })} />
          <TextInput label="Kraj dela" value={form.lokacija} onChange={(lokacija) => setForm({ ...form, lokacija })} />
          <TextInput label="Trajanje" value={form.trajanje} onChange={(trajanje) => setForm({ ...form, trajanje })} />
        </div>
        <label>Opis dela<textarea value={form.opis} onChange={(event) => setForm({ ...form, opis: event.target.value })} /></label>
        <button className="btn-primary" onClick={() => void saveJob()}>{editing ? 'Shrani spremembe' : 'Objavi oglas'}</button>
      </section>
    </DashboardShell>
  );
}

function AdminLogin() {
  const [mail, setMail] = React.useState('admin@studentsko-delo.si');
  const [geslo, setGeslo] = React.useState('');
  const [error, setError] = React.useState('');

  async function login() {
    try {
      const admin = await api.send<{ ime: string }>('/api/Admin/login', 'POST', { mail, geslo });
      localStorage.setItem('admin_logged_in', 'true');
      localStorage.setItem('admin_role', 'admin');
      localStorage.setItem('admin_name', admin.ime);
      go('/admin.html');
    } catch {
      setError('Napačni admin podatki.');
    }
  }

  return (
    <AuthShell>
      <div className="auth-card">
        <h1>Admin prijava</h1>
        <TextInput label="E-pošta" value={mail} onChange={setMail} />
        <TextInput label="Geslo" type="password" value={geslo} onChange={setGeslo} />
        <button className="btn-primary wide" onClick={() => void login()}>Vpiši se</button>
        {error && <p className="error">{error}</p>}
      </div>
    </AuthShell>
  );
}

const entityConfigs: Record<string, EntityConfig> = {
  objave: { label: 'Oglasi', endpoint: '/api/JobOffer', fields: [['ime', 'Naziv dela', 'text'], ['opis', 'Opis', 'textarea'], ['placa', 'Plača', 'number'], ['delovnik', 'Delovnik', 'text'], ['trajanje', 'Trajanje', 'text'], ['podjetja_id', 'ID podjetja', 'number'], ['lokacija', 'Lokacija', 'text'], ['kraj_id', 'ID kraja', 'number']] },
  podjetja: { label: 'Podjetja', endpoint: '/api/Podjetje', createEndpoint: '/api/Podjetje/register', fields: [['ime', 'Ime', 'text'], ['opis', 'Opis', 'textarea'], ['mail', 'E-pošta', 'email'], ['telefon', 'Telefon', 'text'], ['spletna_stran', 'Spletna stran', 'text'], ['geslo', 'Geslo', 'text'], ['kraji_id', 'ID kraja', 'number']] },
  uporabniki: { label: 'Uporabniki', endpoint: '/api/Uporabnik', fields: [['ime', 'Ime', 'text'], ['priimek', 'Priimek', 'text'], ['telefon', 'Telefon', 'text'], ['mail', 'E-pošta', 'email'], ['sola', 'Šola', 'text'], ['geslo', 'Geslo', 'text'], ['cv', 'CV', 'textarea'], ['vrste_uporabnikov_id', 'ID vrste', 'number'], ['kraji_id', 'ID kraja', 'number']] },
  prijave: { label: 'Prijave', endpoint: '/api/Prijava', fields: [['datum_prijave', 'Datum prijave', 'datetime-local'], ['uporabniki_id', 'ID uporabnika', 'number'], ['objave_id', 'ID oglasa', 'number'], ['status', 'Status', 'text']] },
  kraji: { label: 'Kraji', endpoint: '/api/Kraj', fields: [['ime', 'Ime kraja', 'text'], ['postna_st', 'Poštna številka', 'number']] },
  vrste: { label: 'Vrste uporabnikov', endpoint: '/api/VrstaUporabnika', fields: [['ime', 'Ime vrste', 'text'], ['opis', 'Opis', 'textarea']] }
};

function AdminPage() {
  const [active, setActive] = React.useState('objave');
  const [rows, setRows] = React.useState<Record<string, unknown>[]>([]);
  const [editing, setEditing] = React.useState<Record<string, unknown> | null>(null);
  const [form, setForm] = React.useState<Record<string, string>>({});
  const config = entityConfigs[active];

  React.useEffect(() => {
    if (localStorage.getItem('admin_logged_in') !== 'true') go('/admin-prijava.html');
    void loadRows();
  }, [active]);

  async function loadRows() {
    setRows(await api.get<Record<string, unknown>[]>(config.endpoint));
  }

  function edit(row: Record<string, unknown>) {
    setEditing(row);
    const next: Record<string, string> = {};
    config.fields.forEach(([name]) => { next[name] = String(row[name] ?? ''); });
    setForm(next);
  }

  async function save() {
    const data: Record<string, unknown> = editing ? { id: editing.id } : {};
    config.fields.forEach(([name, , type]) => {
      const value = form[name] ?? '';
      data[name] = type === 'number' ? (value === '' ? null : Number(value)) : value;
    });
    await api.send(editing ? `${config.endpoint}/${editing.id}` : (config.createEndpoint || config.endpoint), editing ? 'PUT' : 'POST', data);
    setEditing(null);
    setForm({});
    await loadRows();
  }

  async function remove(id: unknown) {
    if (!confirm('Ali res želite izbrisati zapis?')) return;
    await api.send(`${config.endpoint}/${id}`, 'DELETE', {});
    await loadRows();
  }

  return (
    <DashboardShell title="Administracija podatkov" admin>
      <div className="tabs">
        {Object.entries(entityConfigs).map(([key, item]) => <button className={key === active ? 'active' : ''} key={key} onClick={() => { setActive(key); setEditing(null); setForm({}); }}>{item.label}</button>)}
      </div>
      <section className="admin-layout">
        <div className="panel form-panel">
          <h2>{editing ? 'Uredi zapis' : 'Dodaj zapis'}</h2>
          {config.fields.map(([name, label, type]) => type === 'textarea'
            ? <label key={name}>{label}<textarea value={form[name] || ''} onChange={(event) => setForm({ ...form, [name]: event.target.value })} /></label>
            : <TextInput key={name} label={label} type={type} value={form[name] || ''} onChange={(value) => setForm({ ...form, [name]: value })} />)}
          <button className="btn-primary" onClick={() => void save()}>Shrani</button>
        </div>
        <div className="panel table-panel">
          <h2>{config.label}</h2>
          <DataTable headers={['ID', ...config.fields.map((field) => field[1]), 'Akcije']}>
            {rows.map((row) => (
              <tr key={String(row.id)}>
                <td>{String(row.id)}</td>
                {config.fields.map(([name]) => <td key={name}>{shorten(row[name])}</td>)}
                <td><button onClick={() => edit(row)}>Uredi</button> <button className="danger-btn" onClick={() => void remove(row.id)}>Izbriši</button></td>
              </tr>
            ))}
          </DataTable>
        </div>
      </section>
    </DashboardShell>
  );
}

function StudentsInfo() {
  return <InfoPage title="Pregled študentskega trga dela 2026" subtitle="Informacije za študente" items={['Povprečna postavka: 8,40 €/h', 'Minimalna urna postavka: 7,21 €/h', 'Najbolje plačana dela: IT, inštrukcije, promocije']} />;
}

function CompanyInfo() {
  return <InfoPage title="Informacije za podjetja" subtitle="Podpora delodajalcem" items={['Objavite oglas za študentsko delo', 'Spremljajte prijave kandidatov', 'Sprejmite ali zavrnite prošnjo neposredno v nadzorni plošči']} />;
}

function InfoPage(props: { title: string; subtitle: string; items: string[] }) {
  return (
    <>
      <Header />
      <main className="info-page">
        <span>{props.subtitle}</span>
        <h1>{props.title}</h1>
        <section className="panel">
          {props.items.map((item) => <p key={item}>{item}</p>)}
        </section>
      </main>
    </>
  );
}

function AuthShell({ children }: { children: React.ReactNode }) {
  return (
    <main className="auth-shell">
      <img src="/img/logo.png" alt="Logo" />
      {children}
      <button className="text-button" onClick={() => go('/index.html')}>Nazaj na iskalnik del</button>
    </main>
  );
}

function DashboardShell({ title, children, admin }: { title: string; children: React.ReactNode; admin?: boolean }) {
  function logout() {
    if (admin) {
      localStorage.removeItem('admin_logged_in');
      localStorage.removeItem('admin_role');
      localStorage.removeItem('admin_name');
      go('/admin-prijava.html');
    } else {
      localStorage.clear();
      go('/index.html');
    }
  }

  return (
    <div className="dashboard">
      <aside>
        <h2>{admin ? 'ADMIN' : 'DASHBOARD'}</h2>
        {!admin && <button>Moji oglasi</button>}
        <button onClick={logout}>Odjava</button>
      </aside>
      <main>
        <h1>{title}</h1>
        {children}
      </main>
    </div>
  );
}

function Modal({ title, children, onClose }: { title: string; children: React.ReactNode; onClose: () => void }) {
  return (
    <div className="modal-overlay">
      <div className="modal">
        <header>
          <h2>{title}</h2>
          <button onClick={onClose}>×</button>
        </header>
        <div className="modal-body">{children}</div>
      </div>
    </div>
  );
}

function TextInput(props: { label: string; value: string; onChange: (value: string) => void; type?: string }) {
  return (
    <label>{props.label}
      <input type={props.type || 'text'} value={props.value} onChange={(event) => props.onChange(event.target.value)} />
    </label>
  );
}

function Info(props: { label: string; value: string }) {
  return <div className="detail-item"><strong>{props.label}</strong>{props.value}</div>;
}

function DataTable({ headers, children }: { headers: string[]; children: React.ReactNode }) {
  return (
    <div className="table-wrap">
      <table>
        <thead><tr>{headers.map((header) => <th key={header}>{header}</th>)}</tr></thead>
        <tbody>{children}</tbody>
      </table>
    </div>
  );
}

function shorten(value: unknown) {
  const text = String(value ?? '-');
  return text.length > 70 ? `${text.slice(0, 70)}...` : text;
}

createRoot(document.getElementById('root')!).render(<App />);
