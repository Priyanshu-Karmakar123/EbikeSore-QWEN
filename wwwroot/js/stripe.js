// wwwroot/js/stripe.js
window.initStripePaymentElement = async (publishableKey, clientSecret, returnUrl, email, paymentMethodType) => {
    try {
        // Log parameters to verify inputs
        console.log("Initializing Stripe with parameters:", { publishableKey, clientSecret, returnUrl, email, paymentMethodType });

        // Validate parameters
        if (!publishableKey || typeof publishableKey !== 'string') {
            throw new Error("Invalid or missing Stripe publishable key");
        }
        if (!clientSecret || typeof clientSecret !== 'string') {
            throw new Error("Invalid or missing Stripe client secret");
        }
        if (!returnUrl || typeof returnUrl !== 'string') {
            throw new Error("Invalid or missing return URL");
        }
        if (!paymentMethodType || !['cashapp', 'card'].includes(paymentMethodType)) {
            throw new Error("Invalid or missing payment method type. Must be 'cashapp' or 'card'");
        }

        // Wait for Stripe.js to load
        if (typeof Stripe === 'undefined') {
            // Try to load Stripe.js if not already loaded
            await new Promise((resolve, reject) => {
                const script = document.createElement('script');
                script.src = 'https://js.stripe.com/v3/';
                script.onload = resolve;
                script.onerror = reject;
                document.head.appendChild(script);
            });
        }

        // Initialize Stripe.js
        const stripe = Stripe(publishableKey);
        console.log("Stripe initialized successfully");

        // Wait for DOM to be ready with retries
        let retries = 0;
        const maxRetries = 10;
        let form, paymentElementContainer;
        
        while (retries < maxRetries) {
            form = document.getElementById('payment-form');
            paymentElementContainer = document.getElementById('payment-element');
            
            if (form && paymentElementContainer) {
                break;
            }
            
            await new Promise(resolve => setTimeout(resolve, 100));
            retries++;
        }

        // Verify payment-form and payment-element exist
        if (!form) {
            throw new Error("Payment form (#payment-form) not found in DOM after " + maxRetries + " retries");
        }
        if (!paymentElementContainer) {
            throw new Error("Payment element container (#payment-element) not found in DOM after " + maxRetries + " retries");
        }

        // Set up appearance for Tailwind CSS red theme
        const options = {
            clientSecret: clientSecret,
            appearance: {
                theme: 'stripe',
                variables: {
                    colorPrimary: '#ef4444', // Tailwind red-500
                    colorBackground: '#f9fafb', // Tailwind gray-50
                    colorText: '#111827', // Tailwind gray-900
                    borderRadius: '0.5rem',
                    fontFamily: '"Inter", sans-serif'
                }
            }
        };

        // Create and mount Payment Element
        const elements = stripe.elements(options);
        if (!elements) {
            throw new Error("Failed to create Stripe elements");
        }

        const paymentElementOptions = {
            layout: 'tabs', // Use 'tabs' for consistent UX across payment methods
            defaultValues: {
                billingDetails: { email: email }
            }
        };
        const paymentElement = elements.create('payment', paymentElementOptions);
        if (!paymentElement) {
            throw new Error("Failed to create Payment Element");
        }

        // Clear any existing content in the payment element container
        paymentElementContainer.innerHTML = '';
        
        paymentElement.mount('#payment-element');
        console.log(`Payment Element for ${paymentMethodType} mounted successfully`);

        // Log Payment Element events
        paymentElement.on('ready', () => {
            console.log(`Payment Element for ${paymentMethodType} ready`);
        });
        paymentElement.on('loaderror', (event) => {
            console.error(`Payment Element for ${paymentMethodType} failed to load:`, event.error);
            const messageContainer = document.querySelector('#error-message');
            if (messageContainer) {
                messageContainer.textContent = `Failed to load payment form: ${event.error.message}`;
                messageContainer.classList.remove('hidden');
            }
        });
        paymentElement.on('change', (event) => {
            console.log(`Payment Element change event for ${paymentMethodType}:`, event);
            if (event.error) {
                const messageContainer = document.querySelector('#error-message');
                if (messageContainer) {
                    messageContainer.textContent = `Payment error: ${event.error.message}`;
                    messageContainer.classList.remove('hidden');
                }
            }
        });

        // Handle form submission
        form.addEventListener('submit', async (event) => {
            event.preventDefault();
            const messageContainer = document.querySelector('#error-message');
            setLoading(true);

            try {
                const { error } = await stripe.confirmPayment({
                    elements,
                    confirmParams: {
                        return_url: returnUrl
                    }
                });

                if (error) {
                    console.error(`Payment confirmation error for ${paymentMethodType}:`, error);
                    let errorMessage = error.message;
                    if (error.code === 'payment_intent_payment_attempt_failed') {
                        errorMessage = paymentMethodType === 'card'
                            ? "Payment declined by card issuer. Please try another card."
                            : "Payment declined by Cash App. Please try another payment method.";
                    } else if (error.code === 'payment_intent_redirect_confirmation_without_return_url') {
                        errorMessage = "Payment configuration error. Please contact support.";
                    }
                    if (messageContainer) {
                        messageContainer.textContent = errorMessage;
                        messageContainer.classList.remove('hidden');
                    }
                } else {
                    console.log(`Payment confirmation initiated for ${paymentMethodType}, redirecting...`);
                }
            } catch (error) {
                console.error(`Unexpected error during payment confirmation for ${paymentMethodType}:`, error);
                if (messageContainer) {
                    messageContainer.textContent = `Payment system error: ${error.message}`;
                    messageContainer.classList.remove('hidden');
                }
            } finally {
                setLoading(false);
            }
        });
    } catch (error) {
        console.error(`Error initializing Payment Element for ${paymentMethodType}:`, error);
        const messageContainer = document.querySelector('#error-message');
        if (messageContainer) {
            messageContainer.textContent = `Payment system error: ${error.message}`;
            messageContainer.classList.remove('hidden');
        } else {
            console.error("Error message container not found");
        }
    }
};

window.showStripeSubmitButton = () => {
    console.log("Showing Stripe submit button");
    const submitButton = document.getElementById('submit');
    if (submitButton) {
        submitButton.classList.remove('hidden');
    } else {
        console.error("Submit button not found");
    }
};

window.showErrorMessage = (message) => {
    console.log("Displaying error message:", message);
    const messageContainer = document.querySelector('#error-message');
    if (messageContainer) {
        messageContainer.textContent = message;
        messageContainer.classList.remove('hidden');
    } else {
        console.error("Error message container not found");
    }
};

function setLoading(isLoading) {
    const submitButton = document.getElementById('submit');
    if (submitButton) {
        if (isLoading) {
            submitButton.disabled = true;
            submitButton.innerHTML = '<i class="material-symbols-rounded animate-spin">progress_activity</i><span>Processing...</span>';
        } else {
            submitButton.disabled = false;
            submitButton.innerHTML = '<span>Submit Payment</span>';
        }
    } else {
        console.error("Submit button not found for setLoading");
    }
}