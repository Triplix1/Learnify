export function convertBlobToText(blob: Blob): Promise<string> {
    return new Promise((resolve, reject) => {
        if (blob.type.startsWith('text')) {
            const reader = new FileReader();
            reader.onload = () => resolve(reader.result as string);
            reader.onerror = reject;
            reader.readAsText(blob);
        } else {
            reject('Invalid file type. Expected a text file.');
        }
    });
}