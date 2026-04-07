CREATE TABLE podjetja(
  id serial NOT NULL,
  ime character varying NOT NULL,
  opis text,
  mail character varying NOT NULL,
  telefon integer NOT NULL,
  spletna_stran character varying,
  kraji_id integer NOT NULL,
  CONSTRAINT podjetja_pkey PRIMARY KEY(id)
);


CREATE TABLE uporabniki(
  id serial NOT NULL,
  ime integer NOT NULL,
  priimek integer NOT NULL,
  telefon integer NOT NULL,
  mail character varying NOT NULL,
  sola character varying,
  geslo character varying NOT NULL,
  cv text NOT NULL,
  vrste_uporabnikov_id integer NOT NULL,
  kraji_id integer NOT NULL,
  CONSTRAINT uporabniki_pkey PRIMARY KEY(id)
);


CREATE TABLE vrste_uporabnikov(
  id serial NOT NULL,
  ime character varying NOT NULL,
  opis text,
  CONSTRAINT vrste_uporabnikov_pkey PRIMARY KEY(id)
);


CREATE TABLE objave(
  id serial NOT NULL,
  ime integer NOT NULL,
  opis text NOT NULL,
  placa float8 NOT NULL,
  delovnik character varying NOT NULL,
  trajanje character varying NOT NULL,
  podjetja_id integer NOT NULL,
  CONSTRAINT objave_pkey PRIMARY KEY(id)
);


CREATE TABLE kraji(
  id serial NOT NULL,
  ime character varying NOT NULL,
  postna_st integer NOT NULL,
  CONSTRAINT kraji_pkey PRIMARY KEY(id)
);


CREATE TABLE prijave(
  id serial NOT NULL,
  datum_prijave timestamp NOT NULL,
  uporabniki_id integer NOT NULL,
  objave_id integer NOT NULL,
  CONSTRAINT prijave_pkey PRIMARY KEY(id)
);


ALTER TABLE prijave
  ADD CONSTRAINT prijave_uporabniki_id_fkey
    FOREIGN KEY (uporabniki_id) REFERENCES uporabniki (id)
;


ALTER TABLE prijave
  ADD CONSTRAINT prijave_objave_id_fkey
    FOREIGN KEY (objave_id) REFERENCES objave (id)
;


ALTER TABLE podjetja
  ADD CONSTRAINT podjetja_kraji_id_fkey
    FOREIGN KEY (kraji_id) REFERENCES kraji (id)
;


ALTER TABLE objave
  ADD CONSTRAINT objave_podjetja_id_fkey
    FOREIGN KEY (podjetja_id) REFERENCES podjetja (id)
;


ALTER TABLE uporabniki
  ADD CONSTRAINT uporabniki_vrste_uporabnikov_id_fkey
    FOREIGN KEY (vrste_uporabnikov_id) REFERENCES vrste_uporabnikov (id)
;


ALTER TABLE uporabniki
  ADD CONSTRAINT uporabniki_kraji_id_fkey
    FOREIGN KEY (kraji_id) REFERENCES kraji (id)
;

