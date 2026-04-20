window.indexedDbHelper = {
    dbName: "saludATiempoDb",
    dbVersion: 8,

    openDb: function () {
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(this.dbName, this.dbVersion);

            request.onupgradeneeded = function (event) {
                const db = event.target.result;

                if (db.objectStoreNames.contains("citas")) {
                    db.deleteObjectStore("citas");
                }
                db.createObjectStore("citas", { keyPath: "localId" });

                if (db.objectStoreNames.contains("salud")) {
                    db.deleteObjectStore("salud");
                }
                db.createObjectStore("salud", { keyPath: "localId" });

                if (db.objectStoreNames.contains("tratamientos")) {
                    db.deleteObjectStore("tratamientos");
                }
                db.createObjectStore("tratamientos", { keyPath: "localId" });
            };

            request.onsuccess = function (event) {
                resolve(event.target.result);
            };

            request.onerror = function () {
                reject("Error al abrir IndexedDB");
            };
        });
    },

    getAll: async function (storeName) {
        const db = await this.openDb();

        return new Promise((resolve, reject) => {
            const tx = db.transaction(storeName, "readonly");
            const store = tx.objectStore(storeName);
            const request = store.getAll();

            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject("Error al leer datos");
        });
    },

    put: async function (storeName, item) {
        const db = await this.openDb();

        return new Promise((resolve, reject) => {
            const tx = db.transaction(storeName, "readwrite");
            const store = tx.objectStore(storeName);
            const request = store.put(item);

            request.onsuccess = () => resolve(true);
            request.onerror = () => reject(request.error?.message || "Error al guardar");
        });
    },

    delete: async function (storeName, key) {
        const db = await this.openDb();

        return new Promise((resolve, reject) => {
            const tx = db.transaction(storeName, "readwrite");
            const store = tx.objectStore(storeName);
            const request = store.delete(key);

            request.onsuccess = () => resolve(true);
            request.onerror = () => reject(request.error?.message || "Error al eliminar");
        });
    }
};