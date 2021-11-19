# IceCream Runner

## Project Diary

---

### 19 Novembre 2021

francamente, oggi ho solo un'oretta, vorrei fare/rivedere lo spawn e movimento delle strade, con la FIFO.

---

### 17 Novembre 2021

devo finire la documentazione tecnica, e poi pensare al framework di testing.

#### <span style="color:green">Testing Framework</span>

La cosa che mi ha fermato alla fine era l'impossibilità di testare comodamente le varie componenti del gioco. Per testare, vorrei avere un framework che mi permetta di controllare certi aspetti del gioco in tempo reale. ==Deve essere facile da rimuovere una volta finiti i lavori!==

Dovrebbe implementare le seguenti funzioni:

- modificare la velocità di scorrimento del livello
- fermare ed avviare la velocità di scorrimento del livello
- spawnare una nuova strada davanti alle altre (se non ci sono altre strade nel livello, allora viene spawnata sotto al giocatore)
- eliminare l'ultima strada del livello (non ha senso eliminare strade a caso)
- attivare l'algoritmo di spawn degli Item, su una strada specificata dall'utente
  - deve poter visualizzare e modificare il budget della sezione di strada specificata dall'utente
- fermare ed avviare la l'algoritmo di spawn degli Item automatico sui nuovi segmenti di strada spawnati

##### <span style="color:teal">Cosa voglio fare oggi?</span>

- finire la documentazione tecnica. scrivere la doc funzionale del componente di testing.

##### <span style="color:teal">Cosa ho fatto oggi?</span>

- finita documentazione tecnica (per ora), aggiunto al doc funzionale l'entità Testing Framework :heavy_check_mark:

---

### 13 Novembre 2021

Ho deciso di creare questo diario di progetto per tenere traccia più facilmente dei vari cambiamenti, progressi, intoppi.
Devo sforzarmi di creare una documentazione più completa e manutenuta, per non correre il rishio di perdere il filo degli sviluppi e finire con l'incartarmi e fallire nel produrre il progetto.

<u>So che dovrei darmi una data massima entro cui finire ma... non so se ce la faccio per ora.</u>

La documentazione sarà così suddivisa:

- `[ICR] - Project Diary`: diario di progetto, conterrà pensieri, idee, strategie, problemi ecc.
- `[ICR] - Func Doc`: documento funzionale, descrive ogni componente, le sue funzioni e come si inserisce nel contesto generale del progetto
- `[ICR] - Tech Doc`: documento tecnico, descrive in maniera tecnica proprietà, funzioni, parametri, ecc.

La mia speranza è che così facendo posso lavorare sugli sviluppi con una strategia chiara in mente ed evitare continui rewrite e reboot.

#### <span style="color:green">Perché quest'ultimo rewrite?</span>

L'ultima volta in cui mi sono bloccato, era perché mi era impossibile testare l'algoritmo di popolamento delle strade. Questo perché il modo in cui ho sviluppato la gerarchia è scomodo:

Un oggetto, il manager, viene istanziato e contiene il gameobject `strada_x` e le relative proprietà. Ma per come funziona Unity, se per qualche motivo devo poter accedere alla strada x devo fare un percorso inverso per capire che numero è quella che vedo, ecc.

Vorrei passare a una struttura più semplice: ogni pezzo di strada (GameObject) contiene le sue proprietà ed il `LevelManager` contiene solo il riferimento agli oggetti. Inoltre, vorrei organizzare la lista delle strade in gioco come una FIFO, così che l'indice vada sempre dalla più lontana dal player (la più recente) alla più vicina (la meno recente), scorrendo automaticamente quando un nuovo pezzo di strada viene spawnato.ù

Un altro punto di attenzione sarà fare in modo che ogni componente sia appropriatamente incapsulato, in modo da rendere facile e standardizzata la comunicazione fra script.
Ad esempio, una strada deve esporre l'indice, il budget totale e rimanente, la posizione, la posizione delle celle, il punteggio di ogni cella ecc. in modo che altri script e componenti del sistema possano interrogare ed ottenere queste info facilmente.

##### <span style="color:teal">Cosa voglio fare oggi?</span>

- finire la documentazione funzionale.

##### <span style="color:teal">Cosa ho fatto oggi?</span>

- finita la documentazione funzionale, e iniziato con quella tecnica :heavy_check_mark:

---

