import { isNil } from "lodash";
import { Dispatch, SetStateAction, useCallback, useState } from "react";

export function useLocalStorage<T>(key: string, initialValue: T): [T, Dispatch<SetStateAction<T>>] {
    const [storedValue, setStoredValue] = useState<T>(() => {
        try {
            const item = window.localStorage.getItem(key);

            if (isNil(item)) {
                return initialValue;
            }

            return JSON.parse(item);
        } catch (err) {
            console.error(err);
            return initialValue;
        }
    });

    const setValue = useCallback(
        (value: T | ((oldValue: T) => T)): void => {
            setStoredValue((oldStoredValue) => {
                const valueToStore = value instanceof Function ? value(oldStoredValue) : value;
                try {
                    window.localStorage.setItem(key, JSON.stringify(valueToStore));
                } catch (err) {
                    console.error(err);
                }
                return valueToStore;
            });
        },
        [key, setStoredValue],
    );

    return [storedValue, setValue];
}
