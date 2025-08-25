function initWarehouseToggle() {
    document.querySelectorAll('.warehouse-toggle').forEach(button => {
        button.addEventListener('click', async function () {
            const deviceId = this.dataset.id;
            const deviceType = this.dataset.type;
            const moveToWarehouse = this.dataset.action === 'true';

            try {
                const response = await fetch('/Reports/ToggleWarehouseStatus', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        deviceId,
                        deviceType,
                        moveToWarehouse,
                        note: "İşlem: " + new Date().toLocaleString()
                    })
                });

                if (response.ok) {
                    location.reload();
                }
            } catch (error) {
                console.error('Hata:', error);
            }
        });
    });
}

document.addEventListener('DOMContentLoaded', initWarehouseToggle);