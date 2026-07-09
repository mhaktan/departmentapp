import React, { useState, useEffect } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkDatepicker, TkInput, TkSelect } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

type JobApplicationRecord = {
  id: string | number;
  applicantFirstName: string;
  applicantLastName: string;
  applicantEmail: string;
  applicantPhone?: string;
  coverLetter?: string;
  status: string;
  screeningNotes?: string;
  interviewDate?: string;
  interviewNotes?: string;
  offerSalary?: number;
  offerDate?: string;
  rejectionReason?: string;
  jobPostingId: string;
};

interface JobApplicationEditProps {
  record: JobApplicationRecord | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const JobApplicationEdit: React.FC<JobApplicationEditProps> = ({ record, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<JobApplicationRecord>>(record ?? {});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  useEffect(() => {
    if (record) setForm({ ...record });
  }, [record]);

  const mutation = useMutation({
    mutationFn: (values: Partial<JobApplicationRecord>) =>
      dataProvider.update('JobApplication', record!.id, values),
    onSuccess: (_data, values) => { triggerFlows('update', 'JobApplication', values as Record<string, unknown>); onSuccess(); onClose(); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!record) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Edit JobApplication</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkInput mode="text" label="Applicant First Name *" value={String(form.applicantFirstName ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('applicantFirstName', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Applicant Last Name *" value={String(form.applicantLastName ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('applicantLastName', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Applicant Email *" value={String(form.applicantEmail ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('applicantEmail', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Applicant Phone" value={String(form.applicantPhone ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('applicantPhone', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Cover Letter" value={String(form.coverLetter ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('coverLetter', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Status *" value={String(form.status ?? '')} onChange={(v) => setField('status', v ? Number(v) : null)} searchable={false} options={[{ label: 'Received', value: '0' }, { label: 'Screening', value: '1' }, { label: 'Interview', value: '2' }, { label: 'OfferPending', value: '3' }, { label: 'OfferAccepted', value: '4' }, { label: 'OfferRejected', value: '5' }, { label: 'Rejected', value: '6' }]} />
                </div>
                <div>
                  <TkInput mode="text" label="Screening Notes" value={String(form.screeningNotes ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('screeningNotes', v))(e.detail)} />
                </div>
                <div>
                  <TkDatepicker label="Interview Date" value={String(form.interviewDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('interviewDate', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Interview Notes" value={String(form.interviewNotes ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('interviewNotes', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Offer Salary" value={String(form.offerSalary ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('offerSalary', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkDatepicker label="Offer Date" value={String(form.offerDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('offerDate', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Rejection Reason" value={String(form.rejectionReason ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('rejectionReason', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="İş İlanı *" resource="JobPosting" value={String(form.jobPostingId ?? '')} onChange={(v) => setField('jobPostingId', v)} displayField="title" />
                </div>
            </div>
          </div>
          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8, padding: '16px 28px', borderTop: '1px solid #e8e8e8', flexShrink: 0, background: '#fff' }}>
            <TkButton label="Cancel" variant="secondary" onTkClick={onClose} />
            <TkButton label={mutation.isPending ? 'Saving…' : 'Save Changes'} variant="primary" mode="submit" disabled={mutation.isPending} />
          </div>
        </form>
      </div>
    </div>
  );
};
