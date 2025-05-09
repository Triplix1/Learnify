export const environment = {
    baseApiUrl: (window as any)['env']?.baseApiUrl || 'https://learnify-api.wonderfulbush-ff927553.westeurope.azurecontainerapps.io/api',
    baseMediaUrl: (window as any)['env']?.baseMediaUrl || 'https://learnify-api.wonderfulbush-ff927553.westeurope.azurecontainerapps.io/api/media',
    paymentSuccessUrl: (window as any)['env']?.paymentSuccessUrl || "https://learnify.wonderfulbush-ff927553.westeurope.azurecontainerapps.io/course/payment-success",
    paymentCancelUrl: (window as any)['env']?.paymentCancelUrl || "https://learnify.wonderfulbush-ff927553.westeurope.azurecontainerapps.io/course/payment-cancelled",
    vonageApplicationId: (window as any)['env']?.vonageApplicationId || "af704eed-5133-4e37-8c40-5c1c1b74ba55"
};