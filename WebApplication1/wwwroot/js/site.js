// ============================================
// Museum Tour Management JavaScript
// ============================================

// Toggle FAQ
function toggleFAQ(element) {
    // Close other FAQs
    const allFaqs = document.querySelectorAll('.faq-item');
    allFaqs.forEach(faq => {
        if (faq !== element.parentElement) {
            faq.querySelector('.faq-question').classList.remove('active');
            faq.querySelector('.faq-answer').classList.remove('active');
        }
    });

    // Toggle current FAQ
    const faqItem = element.parentElement;
    const answer = faqItem.querySelector('.faq-answer');

    element.classList.toggle('active');
    answer.classList.toggle('active');
}

// ============================================
// DONATION MODAL FUNCTIONS
// ============================================

// Open Donation Modal
function openDonationModal() {
    const modal = document.getElementById('donation-modal');
    if (modal) {
        modal.style.display = 'block';
    }
}

// Close Donation Modal
function closeDonationModal() {
    const modal = document.getElementById('donation-modal');
    if (modal) {
        modal.style.display = 'none';
    }
}

// ============================================
// DONATION PAGE FUNCTIONS
// ============================================

// Set donation amount from quick buttons
function setAmount(amount) {
    const amountInput = document.getElementById('donation-amount');
    if (amountInput) {
        amountInput.value = amount;
        // Highlight the selected button
        document.querySelectorAll('.amount-btn').forEach(btn => btn.classList.remove('active'));
        event.target.classList.add('active');
    }
}

// Show custom donation amount prompt
function showCustomAmount() {
    const customAmount = prompt('Enter custom amount (£):');
    if (customAmount && !isNaN(customAmount)) {
        document.getElementById('donation-amount').value = customAmount;
        // Remove active class from all buttons
        document.querySelectorAll('.amount-btn').forEach(btn => btn.classList.remove('active'));
    }
}

// ============================================
// DOCUMENT READY - Initialize Event Listeners
// ============================================

document.addEventListener('DOMContentLoaded', function () {

    // -------- Modal Handling --------
    // Close modal when clicking outside of it
    window.onclick = function (event) {
        const modal = document.getElementById('donation-modal');
        if (modal && event.target === modal) {
            closeDonationModal();
        }
    }

    // -------- Home Page Donation Form (Modal) --------
    const donationForm = document.getElementById('donation-form');
    if (donationForm) {
        donationForm.addEventListener('submit', function (e) {
            e.preventDefault();

            const formData = new FormData(donationForm);
            const donationData = {
                firstName: formData.get('donorName') || '',
                lastName: '',
                email: formData.get('donorEmail'),
                phone: '',
                country: '',
                amount: parseFloat(formData.get('amount')),
                paymentMethod: 'card',
                isAnonymous: formData.get('isAnonymous') === 'on',
                giftAid: false,
                subscribeNewsletter: false,
                message: formData.get('message') || '',
                address: '',
                city: '',
                postcode: '',
                cardholderName: '',
                cardNumber: '',
                expiry: '',
                cvv: ''
            };

            // Submit the form to the server
            submitDonationForm(donationData, false);
        });
    }

    // -------- Donation Page Full Form --------
    const donationFormPage = document.getElementById('donation-form-page');
    if (donationFormPage) {
        donationFormPage.addEventListener('submit', function (e) {
            e.preventDefault();

            const formData = new FormData(donationFormPage);

            // Collect all form data
            const donationData = {
                firstName: formData.get('firstName'),
                lastName: formData.get('lastName'),
                email: formData.get('email'),
                phone: formData.get('phone') || '',
                country: formData.get('country'),
                amount: parseFloat(formData.get('amount')),
                paymentMethod: formData.get('paymentMethod'),
                isAnonymous: formData.get('isAnonymous') === 'on',
                giftAid: formData.get('giftAid') === 'on',
                subscribeNewsletter: formData.get('subscribeNewsletter') === 'on',
                message: formData.get('message') || '',
                address: formData.get('address'),
                city: formData.get('city'),
                postcode: formData.get('postcode'),
                cardholderName: formData.get('cardholderName') || '',
                cardNumber: formData.get('cardNumber') || '',
                expiry: formData.get('expiry') || '',
                cvv: formData.get('cvv') || ''
            };

            // Validate required fields
            if (!donationData.firstName || !donationData.lastName || !donationData.email || !donationData.amount) {
                alert('Please fill in all required fields.');
                return;
            }

            // Validate email format
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(donationData.email)) {
                alert('Please enter a valid email address.');
                return;
            }

            // Validate amount
            if (donationData.amount < 1) {
                alert('Please enter an amount of at least £1.');
                return;
            }

            submitDonationForm(donationData, true);
        });
    }

    // -------- Payment Method Toggle --------
    const paymentRadios = document.querySelectorAll('input[name="paymentMethod"]');
    paymentRadios.forEach(radio => {
        radio.addEventListener('change', function () {
            const cardDetails = document.getElementById('card-details');
            if (cardDetails) {
                if (this.value === 'card') {
                    cardDetails.style.display = 'block';
                    // Make card fields required
                    document.getElementById('cardholder-name').required = true;
                    document.getElementById('card-number').required = true;
                    document.getElementById('expiry').required = true;
                    document.getElementById('cvv').required = true;
                } else {
                    cardDetails.style.display = 'none';
                    // Make card fields not required
                    document.getElementById('cardholder-name').required = false;
                    document.getElementById('card-number').required = false;
                    document.getElementById('expiry').required = false;
                    document.getElementById('cvv').required = false;
                }
            }
        });
    });
});

// ============================================
// SUBMIT DONATION FORM
// ============================================

function submitDonationForm(donationData, isFullForm) {
    // Show loading state
    const submitButton = isFullForm
        ? document.querySelector('#donation-form-page button[type="submit"]')
        : document.querySelector('#donation-form button[type="submit"]');

    if (submitButton) {
        submitButton.disabled = true;
        submitButton.textContent = 'Processing...';
    }

    // Send to server
    fetch('/Home/ProcessDonation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
        },
        body: JSON.stringify(donationData)
    })
        .then(response => {
            if (response.ok) {
                // Redirect to success page
                if (isFullForm) {
                    window.location.href = '/Home/Success';
                } else {
                    alert('Thank you for your generous donation!');
                    closeDonationModal();
                    document.getElementById('donation-form').reset();
                }
            } else {
                return response.json().then(data => {
                    throw new Error(data.message || 'Error processing donation');
                });
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('There was an error processing your donation. Please try again.');

            if (submitButton) {
                submitButton.disabled = false;
                submitButton.textContent = isFullForm ? 'CONFIRM PAYMENT' : 'DONATE';
            }
        });
}