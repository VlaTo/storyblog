/**
 * 
 */

const storages: { [key: string]: Storage } = {
    LocalStorage: localStorage,
    SessionStorage: sessionStorage
};

for (var storageTypeName in storages) {
    if (false === storages.hasOwnProperty(storageTypeName)) {
        continue;
    }

    const storage = storages[storageTypeName];
    const alias = `StoryBlog_Web_Blazor_Shared_${storageTypeName}`;

    window[alias] = {
        Count: () => {
            return getLength(storage);
        },

        GetItem: (key: string) => {
            return getItem(storage, key);
        },

        SetItem: (key: string, data: string) => {
            setItem(storage, key, data);
        },

        RemoveItem: (key: string) => {
            removeItem(storage, key);
        },

        Clear: () => {
            clear(storage);
        }
    }
}

function getLength(storage: Storage) {
    return storage.length;
}

function setItem(storage: Storage, key: string, data: string) {
    storage.setItem(key, data);
}

function getItem(storage: Storage, key: string) {
    return storage.getItem(key);
}

function removeItem(storage: Storage, key: string) {
    storage.removeItem(key);
}

function clear(storage: Storage) {
    storage.clear();
}