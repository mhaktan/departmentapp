import React, { useState } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkDatepicker, TkInput, TkSelect } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

type PerformanceGoalRecord = {
  id: string | number;
  title: string;
  description?: string;
  targetDate?: string;
  weight?: number;
  selfScore?: number;
  managerScore?: number;
  status: string;
  performanceReviewId: string;
};

interface PerformanceGoalCreateProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const PerformanceGoalCreate: React.FC<PerformanceGoalCreateProps> = ({ open, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<PerformanceGoalRecord>>({});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  const mutation = useMutation({
    mutationFn: (values: Partial<PerformanceGoalRecord>) => dataProvider.create('PerformanceGoal', values),
    onSuccess: (_data, values) => { triggerFlows('create', 'PerformanceGoal', values as Record<string, unknown>); onSuccess(); onClose(); setForm({}); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!open) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Create PerformanceGoal</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkInput mode="text" label="Title *" value={String(form.title ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('title', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Description" value={String(form.description ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('description', v))(e.detail)} />
                </div>
                <div>
                  <TkDatepicker label="Target Date" value={String(form.targetDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('targetDate', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Weight" value={String(form.weight ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('weight', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Self Score" value={String(form.selfScore ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('selfScore', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Manager Score" value={String(form.managerScore ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('managerScore', Number(v)))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Status *" value={String(form.status ?? '')} onChange={(v) => setField('status', v ? Number(v) : null)} searchable={false} options={[{ label: 'Active', value: '0' }, { label: 'Completed', value: '1' }, { label: 'Cancelled', value: '2' }]} />
                </div>
                <div>
                  <LookupSelect label="Performans Değerlendirmesi *" resource="PerformanceReview" value={String(form.performanceReviewId ?? '')} onChange={(v) => setField('performanceReviewId', v)} displayField="reviewPeriod" />
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
