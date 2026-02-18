// Museum Tour Management JavaScript

// Toggle FAQ
function toggleFAQ(element) {
    const answer = element.nextElementSibling;
    element.classList.toggle('active');
    answer.classList.toggle('active');
}

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

// Handle Donation Form Submit
document.addEventListener('DOMContentLoaded', function () {
    // Close modal when clicking outside
    window.onclick = function (event) {
        const modal = document.getElementById('donation-modal');
        if (event.target === modal) {
            closeDonationModal();
        }
    }

    // Handle donation form
    const donationForm = document.getElementById('donation-form');
    if (donationForm) {
        donationForm.addEventListener('submit', async function (e) {
            e.preventDefault();

            const formData = new FormData(donationForm);
            const donationData = {
                donorName: formData.get('donorName'),
                donorEmail: formData.get('donorEmail'),
                amount: parseFloat(formData.get('amount')),
                message: formData.get('message') || '',
                isAnonymous: formData.get('isAnonymous') === 'on'
            };

            try {
                const response = await fetch('/Donations/Create', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(donationData)
                });

                if (response.ok) {
                    alert('Thank you for your generous donation!');
                    closeDonationModal();
                    donationForm.reset();
                } else {
                    alert('There was an error processing your donation. Please try again.');
                }
            } catch (error) {
                console.error('Error:', error);
                alert('There was an error processing your donation. Please try again.');
            }
        });
    }
});