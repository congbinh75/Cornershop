import { initializeApp } from "firebase/app";
import { getStorage } from "firebase/storage";

import { ref, getDownloadURL, uploadBytesResumable } from "firebase/storage";

const firebaseConfig = {
  apiKey: import.meta.env.VITE_FIREBASE_API_KEY,
  authDomain: import.meta.env.VITE_FIREBASE_AUTH_DOMAIN,
  projectId: import.meta.env.VITE_FIREBASE_PROJECT_ID,
  storageBucket: import.meta.env.VITE_FIREBASE_STORAGE_BUCKET,
  messagingSenderId: import.meta.env.VITE_FIREBASE_PASSWORD,
  appId: import.meta.env.VITE_FIREBASE_APP_ID,
};

const app = initializeApp(firebaseConfig);
const storage = getStorage(app);

export const UploadImage = async (file: File): Promise<string | undefined> => {
  if (!file) return undefined;

  return new Promise<string | undefined>((resolve, reject) => {
    const storageRef = ref(storage, `images/products/${file.name}`);
    const uploadTask = uploadBytesResumable(storageRef, file);

    uploadTask.on(
      "state_changed",
      (snapshot) => {
        const progress = Math.round(
          (snapshot.bytesTransferred / snapshot.totalBytes) * 100
        );
        console.log(progress);
      },
      (error) => {
        console.error("Upload failed:", error);
        reject(undefined);
      },
      () => {
        getDownloadURL(uploadTask.snapshot.ref).then(
          (downloadURL) => {
            resolve(downloadURL);
          },
          (error) => {
            console.error("Failed to get download URL:", error);
            resolve(undefined);
          }
        );
      }
    );
  });
};
