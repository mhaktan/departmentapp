import React, { useState } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkDatepicker, TkInput, TkSelect } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

type TrainingRecord = {
  id: string | number;
  title: string;
  provider?: string;
  startDate?: string;
  endDate?: string;
  location?: string;
  trainingType: string;
  status: string;
  cost?: number;
  currency?: string;
  trainingPlanId: string;
};

interface TrainingCreateProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const TrainingCreate: React.FC<TrainingCreateProps> = ({ open, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<TrainingRecord>>({});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  const mutation = useMutation({
    mutationFn: (values: Partial<TrainingRecord>) => dataProvider.create('Training', values),
    onSuccess: (_data, values) => { triggerFlows('create', 'Training', values as Record<string, unknown>); onSuccess(); onClose(); setForm({}); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!open) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Create Training</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkInput mode="text" label="Title *" value={String(form.title ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('title', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Provider" value={String(form.provider ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('provider', v))(e.detail)} />
                </div>
                <div>
                  <TkDatepicker label="Start Date" value={String(form.startDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('startDate', v))(e.detail)} />
                </div>
                <div>
                  <TkDatepicker label="End Date" value={String(form.endDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('endDate', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Location" value={String(form.location ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('location', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Training Type *" value={String(form.trainingType ?? '')} onChange={(v) => setField('trainingType', v ? Number(v) : null)} searchable={false} options={[{ label: 'Internal', value: '0' }, { label: 'External', value: '1' }, { label: 'Online', value: '2' }, { label: 'OnTheJob', value: '3' }]} />
                </div>
                <div>
                  <LookupSelect label="Status *" value={String(form.status ?? '')} onChange={(v) => setField('status', v ? Number(v) : null)} searchable={false} options={[{ label: 'Planned', value: '0' }, { label: 'Ongoing', value: '1' }, { label: 'Completed', value: '2' }, { label: 'Cancelled', value: '3' }]} />
                </div>
                <div>
                  <TkInput mode="number" label="Cost" value={String(form.cost ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('cost', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Currency" value={String(form.currency ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('currency', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Eğitim Planı *" resource="TrainingPlan" value={String(form.trainingPlanId ?? '')} onChange={(v) => setField('trainingPlanId', v)} displayField="title" />
                </div>
            </div>
          </div>
          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8, padding: '16px 28px', borderTop: '1px solid #e8e8e8', flexShrink: 0, background: '#fff' }}>
            <TkButton label="Cancel" variant="secondary" onTkClick={onClose} />
            <TkButton label={mutation.isPending ? 'Saving…' : 'Create'} variant="primary" mode="submit" disabled={mutation.isPending} />
          </div>
        </form>
      </div>
    </div>
  );
};
